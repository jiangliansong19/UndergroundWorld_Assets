using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

/// <summary>
/// 建筑
/// </summary>
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { private set; get; }

    [SerializeField] private Transform[] treeTransforms;

    private BuildingTypeSO activeBuildingTypeSO;

    [HideInInspector] public Dictionary<BuildingTypeSO, GameObject> buildingInfoDict;

    private GameObject BuildingInfoDialog;

    

    public event EventHandler<OnActiveBuildingTypeChangedHandlerArgs> OnActiveBuildingTypeChangedHandler;

    public class OnActiveBuildingTypeChangedHandlerArgs
    {
        public BuildingTypeSO Args_TypeSO;
    }




    private void Awake()
    {
        Instance = this;
        buildingInfoDict = new Dictionary<BuildingTypeSO, GameObject>();

    }

    // Start is called before the first frame update
    void Start()
    {
        CreateWorld();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && activeBuildingTypeSO != null && !EventSystem.current.IsPointerOverGameObject())
        {
            
            if (!activeBuildingTypeSO.continuousBuild)
            {
                generateABuilding(activeBuildingTypeSO, UtilsClass.getRoundCurrentWorldPoint());
            }
        }


        //cancel
        if (Input.GetMouseButtonDown(1))
        {
            SetActiveBuildingTypeSO(null);
        }

        //test
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Click on G to generate man");

            WorkingManager.Instance.AddWorker(Worker.GenerateAWorker(UtilsClass.GetCurrentWorldPoint()));
        }
    }




    public void SetActiveBuildingTypeSO(BuildingTypeSO typeSO)
    {
        activeBuildingTypeSO = typeSO;
        OnActiveBuildingTypeChangedHandler?.Invoke(this, new OnActiveBuildingTypeChangedHandlerArgs
        {
            Args_TypeSO = activeBuildingTypeSO
        });

        StartObserveRectRender();
    }

    public BuildingTypeSO GetActiveBuildingTypeSO()
    {
        return activeBuildingTypeSO;
    }



    public void ScanBuilding()
    {
        Vector3 position = UtilsClass.GetCurrentWorldPoint();
        GameObject obj = UtilsClass.GetObjectByRay(position);
        if (obj != null && obj.tag.StartsWith("Building"))
        {
            //ShowBuildingInfoDialog(obj, position);
            BuildingRunData runData = obj.GetComponent<BuildingRunData>();
            BuildingTypeSOHolder holder = obj.GetComponent<BuildingTypeSOHolder>();

            string message = holder.buidlingTypeSO.buildingName + "\n" + runData.GetBuildingDescription();
            ToolTipsUI.Instance.Show(message, new ToolTipsUI.TooltipTimer { timer = 3 });
        }
    }

    public void generateABuilding(BuildingTypeSO buildingTypeSO, Vector2 position)
    {
        //position is has other object
        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.transform.GetComponent<BoxCollider2D>();
        Collider2D[] boxColliders = Physics2D.OverlapBoxAll((Vector3)position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        if (boxColliders != null && boxColliders.Length > 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
            return;
        }

        Vector2 correctBuildPosition = position;
        if (buildingTypeSO.buildOnSoil)
        {
            RaycastHit2D rayCastObj = Physics2D.Raycast(position, new Vector2(0, -1).normalized);
            if (rayCastObj.collider.tag == "Soil")
            {
                if (position.y - rayCastObj.transform.position.y - boxCollider2D.size.y/2.0 - 0.5f > 1)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                    return;
                }
                else
                {
                    for (float i = -buildingTypeSO.prefab.transform.localScale.x /2 ; i <= -buildingTypeSO.prefab.transform.localScale.x / 2; i++)
                    {

                    }
                }
            }

        }

        //can afford enough resources
        bool canAfford = true;
        foreach (ResourceTypeAmount item in buildingTypeSO.constructionCosts)
        {
            if (!ResourcesManager.Instance.CanAffordResource(item.resourceType.type, item.amount))
            {
                ToolTipsUI.Instance.Show("can not afford " + item.resourceType.nameString + " " + item.amount,
                                            new ToolTipsUI.TooltipTimer() { timer = 2f }, null);
                canAfford = false;
                SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                return;
            }
        }






        Vector3? correctPosition = GetCorrectBuildPosition(buildingTypeSO.prefab.transform, position);
        if (correctPosition == null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
            return;
        }



        if (canAfford) 
        {
            //craete
            Transform newObj = Instantiate(buildingTypeSO.prefab, (Vector3)correctPosition, Quaternion.identity);

            //building construction
            foreach (ResourceTypeAmount item in buildingTypeSO.constructionCosts) 
            {
                ResourcesManager.Instance.AddResource(item.resourceType.type, -(int)item.amount);
            }

            //addresource
            bool everyCycle = buildingTypeSO.type != BuildingType.House;
            ResourceType resourceType = buildingTypeSO.generateResource.resourceType.type;
            long amount = buildingTypeSO.generateResource.amount;
            ResourcesManager.Instance.AddResourcePerCycle(resourceType, amount, everyCycle);

            //spend workes
            ResourcesManager.Instance.AddResourcePerCycle(ResourceType.Worker, -buildingTypeSO.workersNumber, false);
        }


    }


    public void DestructBuilding(GameObject obj, Vector3 position)
    {
        GameObject gameObj = UtilsClass.GetObjectByRay(position);
        if (gameObj.GetComponent<BuildingTypeSO>() != null)
        {
            Destroy(gameObj);
        }
        
    }

    public void UpdateBuilding(GameObject obj, Vector3 position)
    {

    }


    public void ShowBuildingInfoDialog(GameObject building, Vector3 position)
    {
        if (BuildingInfoDialog == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Others/BuildingInfoDialog").gameObject;
            BuildingInfoDialog = Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            BuildingInfoDialog.transform.position = position;
        }

        BuildingInfoDialog.GetComponent<SpriteRenderer>().sortingOrder = building.GetComponent<SpriteRenderer>().sortingOrder + 1;
        BuildingRunData data = building.GetComponent<BuildingRunData>();
        BuildingInfoDialog.transform.Find("Content").GetComponent<TMPro.TextMeshPro>().SetText(data.incomePerDay.ToString());
    }





    private Vector3? GetCorrectBuildPosition(Transform trans, Vector3 clickPosition)
    {

        BoxCollider2D boxCollider2D = trans.GetComponent<BoxCollider2D>();
        Collider2D[] boxColliders = Physics2D.OverlapBoxAll(clickPosition + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);


        if (activeBuildingTypeSO.buildOnSoil == false)
        {
            return clickPosition;
        }


        RaycastHit2D rayCastObj = Physics2D.Raycast(clickPosition, new Vector2(0, -1));
        if (rayCastObj.collider.tag == "Soil")
        {
            
            BoxCollider2D boxCollider = trans.GetComponent<BoxCollider2D>();
            float soilColliderHeight = 1f;

            float collidersDistance = boxCollider.size.y / 2 + soilColliderHeight / 2;
            float positionDistance = clickPosition.y - rayCastObj.collider.transform.position.y;

            if (positionDistance == collidersDistance)
            {
                return clickPosition;
            }
            else if (positionDistance - collidersDistance > 0 && positionDistance - collidersDistance < 1)
            {
                return new Vector3(clickPosition.x, clickPosition.y - 0.5f, 0);
            }
            else if (positionDistance - collidersDistance > 1)
            {
                return null;
            }
            else if (positionDistance - collidersDistance < 0)
            {
                return null;
            }
        }
        else
        {
            return null;
        }
        return null;
    }








    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private void StartObserveRectRender()
    {
        if (activeBuildingTypeSO == null)
        {
            return;
        }

        if (!activeBuildingTypeSO.continuousBuild)
        {
            return;
        }

        RectRender rectRender = Camera.main.GetComponent<RectRender>();
        rectRender.OnDrawRectStartPosition += (object sender, RectRender.RectRenderEventHandlerArgs e) =>
        {
            _startPosition = e.position;
        };
        rectRender.OnDrawRectEndPosition += (object sender, RectRender.RectRenderEventHandlerArgs e) =>
        {
            _endPosition = e.position;

            //could continuos to build
            if (activeBuildingTypeSO != null)
            {
                CheckToContinuousBuild();
            }
        };
    }


    private void CheckToContinuousBuild()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(_startPosition, _endPosition);
        if (colliders.Length == 0)
        {
            for (float i = Mathf.Min(_startPosition.x, _endPosition.x); i < Mathf.Max(_startPosition.x, _endPosition.x); i++)
            {
                for (float j = Mathf.Min(_startPosition.y, _endPosition.y); j < Mathf.Max(_startPosition.y, _endPosition.y); j++)
                {
                    Vector2 cur = new Vector2(Mathf.RoundToInt(i), Mathf.RoundToInt(j));
                    Instantiate(activeBuildingTypeSO.prefab, cur, Quaternion.identity);
                }
            }
        }
    }














    private void CreateWorld()
    {
        Transform waterTransform = Resources.Load<Transform>("pfWater");
        Transform waterTransform1 = Resources.Load<Transform>("pfWater1");
        Transform waterTransform2 = Resources.Load<Transform>("pfWater2");
        Transform soilTransform = Resources.Load<Transform>("pfSoil");

        Vector2[] lake1 = new Vector2[3] { new Vector2(-8, 0) , new Vector2(-7,0), new Vector2(-7,-1) };
        //Vector2[] lake1 = new Vector2[] { new Vector2(-8, 0), new Vector2(-7, 0), new Vector2(-7, -1) };

        int[,] destructSoils = new int[10, 2]
        {
            {0, -1},
            {0, -2},
            {0, -3}, {1, -3}, {2, -3}, {3, -3},
            {0, -4}, {1, -4}, {2, -4}, {3, -4}
        };

        Vector2[] empty1 = new Vector2[10];
        for (int i = 0; i < 10; i++)
        {
            empty1[i] = new Vector2(destructSoils[i, 0], destructSoils[i, 1]);
        }



        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 0; j++)
            {
                Vector2 position = new Vector2(i, j);
                if (lake1.Contains(position)) 
                {
                    Instantiate(waterTransform, position, Quaternion.identity);
                }
                else if (empty1.Contains(position)) {

                }
                else
                {
                    Instantiate(soilTransform, position, Quaternion.identity);

                }
            }

            //create trees
            if (i % 8 == 0 && (i < 0 || i > 3))
            {
                Transform tree = treeTransforms[Random.Range(0, treeTransforms.Length)];
                Vector3 treePosition = new Vector3(i + Random.Range(0, 7), -0.5f, 0);
                Instantiate(tree, treePosition, Quaternion.identity);
            }
        }

        //create ladder
        for (int j = -4; j < 0; j++)
        {
            GameObject ladder = Resources.Load<GameObject>("pfLadder");
            Instantiate(ladder, new Vector2(0, j), Quaternion.identity);
        }

        //create a small house as area center
        BottomMenuSO menuSO = Resources.Load<BottomMenuSO>(typeof(BottomMenuSO).Name);
        Transform house = Instantiate(menuSO.menuList[1].buildingList[0].prefab, new Vector2(2, -3.5f), Quaternion.identity);

        ResourcesManager.Instance.AddResource(ResourceType.Worker, 4);
        
    }
}
