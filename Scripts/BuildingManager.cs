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
            
            if (activeBuildingTypeSO.type == BuildingType.Road)
            {

            }
            else
            {
                BuildBuilding();
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

            Transform man = Resources.Load<Transform>("pfMan");
            Transform obj = Instantiate(man, UtilsClass.GetCurrentWorldPoint(), Quaternion.identity);

            List<Worker> workers = new List<Worker>();
            workers.Add(obj.GetComponent<Worker>());
            WorkingManager.Instance.AddWorkers(workers);
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

    public void BuildBuilding()
    {
        Vector3? correctPosition = GetCorrectBuildPosition(activeBuildingTypeSO.prefab.transform);
        if (correctPosition == null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
            return;
        }

        if (ResourcesManager.Instance.CanAffordResourceAmounts(activeBuildingTypeSO.constructionCosts)) 
        {
            Transform newObj = Instantiate(this.activeBuildingTypeSO.prefab, (Vector3)correctPosition, Quaternion.identity);
            ResourcesManager.Instance.SpendResourceAmounts(activeBuildingTypeSO.constructionCosts);




            //ResourceTypeAmount resourceAmount = activeBuildingTypeSO.generateResource;
            //if (resourceAmount.resourceType != null && resourceAmount.amount != 0)
            //{
            //    ResourceGeneratorManager.Instance.AddResourcePerCycle(resourceAmount);
            //}

        }
        else
        {

        }
        


    }

    /// <summary>
    /// 销毁建筑
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="position"></param>
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

    /// <summary>
    /// 查看建筑的详情
    /// </summary>
    /// <param name="building"></param>
    /// <param name="position"></param>
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





    private Vector3? GetCorrectBuildPosition(Transform trans)
    {
        Vector3 mousePoint = UtilsClass.getRoundCurrentWorldPoint();

        BoxCollider2D boxCollider2D = trans.GetComponent<BoxCollider2D>();
        Collider2D[] boxColliders = Physics2D.OverlapBoxAll(mousePoint + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        Debug.LogFormat("11OverlapBoxAll = {0}", boxColliders.Length);

        if (activeBuildingTypeSO.buildOnSoil == false)
        {
            return mousePoint;
        }


        RaycastHit2D rayCastObj = Physics2D.Raycast(mousePoint, new Vector2(0, -1));
        if (rayCastObj.collider.tag == "Soil")
        {
            
            BoxCollider2D boxCollider = trans.GetComponent<BoxCollider2D>();
            float soilColliderHeight = 1f;

            float collidersDistance = boxCollider.size.y / 2 + soilColliderHeight / 2;
            float positionDistance = mousePoint.y - rayCastObj.collider.transform.position.y;

            if (positionDistance == collidersDistance)
            {
                return mousePoint;
            }
            else if (positionDistance - collidersDistance > 0 && positionDistance - collidersDistance < 1)
            {
                return new Vector3(mousePoint.x, mousePoint.y - 0.5f, 0);
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

        if (activeBuildingTypeSO.type != BuildingType.Road)
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
            if (activeBuildingTypeSO != null && activeBuildingTypeSO.type == BuildingType.Road)
            {
                CheckToBuildRoads();
            }
        };
    }


    private void CheckToBuildRoads()
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

        for (int i = -50; i < 50; i++)
        {
            for (int j = -50; j < 0; j++)
            {
                Vector2 position = new Vector2(i, j);
                if (lake1.Contains(position)) 
                {
                    Instantiate(waterTransform, position, Quaternion.identity);
                }
                else
                {
                    Instantiate(soilTransform, position, Quaternion.identity);
                }
                
            }

            if (i % 8 == 0)
            {
                Transform tree = treeTransforms[Random.Range(0, treeTransforms.Length)];
                Vector3 treePosition = new Vector3(i + Random.Range(0, 7), -0.5f, 0);
                Instantiate(tree, treePosition, Quaternion.identity);
            }
        }
    }
}
