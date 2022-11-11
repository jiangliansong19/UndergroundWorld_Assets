using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { private set; get; }

    [HideInInspector] public Dictionary<BuildingTypeSO, GameObject> buildingInfoDict;

    [SerializeField] private Transform[] treeTransforms;

    private BuildingTypeSO activeBuildingTypeSO;

    private GameObject _buildingInfoDialog;

    private Transform _centerTransform;
    

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
                generateBuilding(activeBuildingTypeSO, UtilsClass.getRoundCurrentWorldPoint());
            }
        }

        //show building info dialog
        if (Input.GetMouseButtonDown(0) && activeBuildingTypeSO == null && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject obj = UtilsClass.GetObjectByRay(UtilsClass.GetCurrentWorldPoint());
            if (obj != null && obj.transform != null && obj.GetComponent<BuildingTypeSOHolder>() != null)
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



    public void generateBuilding(BuildingTypeSO buildingTypeSO, Vector2 position)
    {
        Debug.Log("genereate building position : " + position.ToString());

        //检测范围内是否有其他建筑
        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.transform.GetComponent<BoxCollider2D>();
        Collider2D[] boxColliders = Physics2D.OverlapBoxAll((Vector3)position + (Vector3)boxCollider2D.offset, boxCollider2D.size - new Vector2(0.1f, 0.1f), 0);
        if (boxColliders != null && boxColliders.Length > 0)
        {
            Debug.Log("position is has other object");
            //SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
            return;
        }

        //是否能建在土地上
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
                Debug.Log("bottom is not soil");
                //SoundManager.Instance.PlaySound(SoundManager.Sound.ClickError);
                return;
            }
        }

        //是否能支付相应的资源。
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


        //建造建筑
        Transform newObj = Instantiate(buildingTypeSO.prefab, (Vector3)correctBuildPosition, Quaternion.identity);

        //消耗资源
        foreach (ResourceTypeAmount item in buildingTypeSO.constructionCosts)
        {
            ResourcesManager.Instance.AddResource(item.resourceType.type, -(int)item.amount);
        }

        //生产型建筑，每个循环周期都会增加资源
        if (buildingTypeSO.IsResourceGenerator())
        {
            ResourceType resourceType = buildingTypeSO.generateResource.resourceType.type;
            bool everyCycle = resourceType != ResourceType.Worker;
            long amount = buildingTypeSO.generateResource.amount;
            ResourcesManager.Instance.AddResourcePerCycle(resourceType, amount, everyCycle);
        }

        //需要工人的建筑，消耗工人数量
        ResourcesManager.Instance.AddResourcePerCycle(ResourceType.Worker, -buildingTypeSO.workersNumber, false);


        //建筑是否连接到地区中心
        bool isConnected = PathFinder.Instance.isConnectBetween(newObj, _centerTransform);
        Transform warning = newObj.Find("Warning");
        if (warning != null)
        {
            warning.gameObject.SetActive(!isConnected);
        }
    }



    public void UpdateBuilding(GameObject obj, Vector3 position)
    {

    }

    /// <summary>
    /// 展示建筑信息
    /// </summary>
    /// <param name="building">建筑</param>
    /// <param name="position">位置</param>
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



    //检测鼠标绘制的矩形区域，判断是否可以连续建造
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

        TouchEventHandler eventHandler = Camera.main.GetComponent<TouchEventHandler>();
        eventHandler.OnTouchBegin += (object sender, TouchEventHandler.TouchEventArgs e) =>
        {
            _startPosition = e.position;
        };
        eventHandler.OnTouchEnd += (object sender, TouchEventHandler.TouchEventArgs e) =>
        {
            _endPosition = e.position;

            //could continuos to build
            if (activeBuildingTypeSO != null)
            {
                CheckToContinuousBuild();
            }
        };
    }

    //判断是否可以连续建造
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
                    generateBuilding(activeBuildingTypeSO, cur);
                }
            }
        }
    }













    /// <summary>
    /// 游戏启动时，创建的初始世界
    /// </summary>
    private void CreateWorld()
    {
        Transform waterTransform = Resources.Load<Transform>("pfWater");
        Transform waterTransform1 = Resources.Load<Transform>("pfWater1");
        Transform waterTransform2 = Resources.Load<Transform>("pfWater2");
        Transform soilTransform = Resources.Load<Transform>("pfSoil");

        Vector2[] lake1 = new Vector2[3] { new Vector2(-8, -1) , new Vector2(-7, -1), new Vector2(-7, -2) };
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

        //position的原点在左上角。

        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 0; j++)
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
        _centerTransform = Instantiate(menuSO.menuList[1].buildingList[0].prefab, new Vector2(2, -4.0f), Quaternion.identity);

        ResourcesManager.Instance.AddResource(ResourceType.Worker, 4);

        
    }
}
