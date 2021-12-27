using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { private set; get; }

    [SerializeField] private Transform[] treeTransforms;

    private BuildingTypeSO activeBuildingTypeSO;

    [HideInInspector] public Dictionary<BuildingTypeSO, GameObject> buildingInfoDict;

    private GameObject _buildingInfoDialog;

    

    public event EventHandler<OnActiveBuildingTypeChangedHandlerArgs> OnActiveBuildingTypeChangedHandler;
    public class OnActiveBuildingTypeChangedHandlerArgs {
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
        //generate a building
        if (Input.GetMouseButtonDown(0) && activeBuildingTypeSO != null && !EventSystem.current.IsPointerOverGameObject())
        {
            
            if (!activeBuildingTypeSO.continuousBuild)
            {
                generateABuilding(activeBuildingTypeSO, UtilsClass.getRoundCurrentWorldPoint());
            }
        }

        //show building info dialog
        if (Input.GetMouseButtonDown(0) && activeBuildingTypeSO == null && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject obj = UtilsClass.GetObjectByRay(UtilsClass.GetCurrentWorldPoint());
            if (obj != null && obj.transform != null)
            {
                BuildingInfoDialog.Instance.ShowBuildingInfo(obj.transform);
            }
        }

        //exit building style
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



    public void generateABuilding(BuildingTypeSO buildingTypeSO, Vector2 position)
    {
        Debug.Log("genereate building position : " + position.ToString());

        //position is has other object
        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.transform.GetComponent<BoxCollider2D>();
        Collider2D[] boxColliders = Physics2D.OverlapBoxAll((Vector3)position + (Vector3)boxCollider2D.offset, boxCollider2D.size - new Vector2(0.1f, 0.1f), 0);
        if (boxColliders != null && boxColliders.Length > 0)
        {
            Debug.Log("position is has other object");
            //SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
            return;
        }

        //must build on soil
        Vector2 correctBuildPosition = position;
        if (buildingTypeSO.buildOnSoil)
        {
            RaycastHit2D rayCastObj = Physics2D.Raycast(position, new Vector2(0, -1).normalized);
            if (rayCastObj.collider.tag == "Soil")
            {
                if (position.y - rayCastObj.transform.position.y - boxCollider2D.size.y/2.0 - 0.5f > 1)
                {
                    //SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                    return;
                }
                else
                {
                    for (float i = position.x - boxCollider2D.size.x / 2 + 0.5f; i < position.x + boxCollider2D.size.x / 2; i++)
                    {
                        Vector2 rayStartPosition = new Vector2(i, position.y);
                        RaycastHit2D rayCastObj_N = Physics2D.Raycast(rayStartPosition, new Vector2(0, -1).normalized);
                        if (rayCastObj_N.collider.tag == "Soil")
                        {
                            if (position.y - rayCastObj_N.transform.position.y - boxCollider2D.size.y / 2.0 - 0.5f > 1)
                            {
                                SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                                return;
                            }
                        }
                    }
                }
                correctBuildPosition = new Vector2(position.x, rayCastObj.transform.position.y + 0.5f + boxCollider2D.size.y/2.0f);
            }
            else
            {
                Debug.Log("bottom is is not soil");
                //SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                return;
            }
        }

        //can afford enough resources
        foreach (ResourceTypeAmount item in buildingTypeSO.constructionCosts)
        {
            if (!ResourcesManager.Instance.CanAffordResource(item.resourceType.type, item.amount))
            {
                ToolTipsUI.Instance.Show("can not afford " + item.resourceType.nameString + " " + item.amount,
                                            new ToolTipsUI.TooltipTimer() { timer = 2f }, null);
                //SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                return;
            }
        }


        //craete
        Transform newObj = Instantiate(buildingTypeSO.prefab, (Vector3)correctBuildPosition, Quaternion.identity);

        //building construction
        foreach (ResourceTypeAmount item in buildingTypeSO.constructionCosts)
        {
            ResourcesManager.Instance.AddResource(item.resourceType.type, -(int)item.amount);
        }

        //addresource
        if (buildingTypeSO.IsResourceGenerator())
        {
            ResourceType resourceType = buildingTypeSO.generateResource.resourceType.type;
            bool everyCycle = resourceType != ResourceType.Worker;
            long amount = buildingTypeSO.generateResource.amount;
            ResourcesManager.Instance.AddResourcePerCycle(resourceType, amount, everyCycle);
        }

        //spend workes
        ResourcesManager.Instance.AddResourcePerCycle(ResourceType.Worker, -buildingTypeSO.workersNumber, false);

    }



    public void UpdateBuilding(GameObject obj, Vector3 position)
    {

    }


    public void ShowBuildingInfoDialog(GameObject building, Vector3 position)
    {
        if (_buildingInfoDialog == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Others/BuildingInfoDialog").gameObject;
            _buildingInfoDialog = Instantiate(prefab, position, Quaternion.identity);
        }
        else
        {
            _buildingInfoDialog.transform.position = position;
        }

        _buildingInfoDialog.GetComponent<SpriteRenderer>().sortingOrder = building.GetComponent<SpriteRenderer>().sortingOrder + 1;
        BuildingRunData data = building.GetComponent<BuildingRunData>();
        _buildingInfoDialog.transform.Find("Content").GetComponent<TMPro.TextMeshPro>().SetText(data.incomePerDay.ToString());
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
                    generateABuilding(activeBuildingTypeSO, cur);
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
        Transform house = Instantiate(menuSO.menuList[1].buildingList[0].prefab, new Vector2(2, -4.0f), Quaternion.identity);

        ResourcesManager.Instance.AddResource(ResourceType.Worker, 4);

        
    }
}
