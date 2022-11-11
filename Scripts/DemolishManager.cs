using System.Collections.Generic;
using UnityEngine;

public enum DemolishType
{
    None,
    Digging,
    CutTree,
    PullDown,
    Collect,
    Update,
} 



public class DemolishManager : MonoBehaviour
{
    public static DemolishManager Instance { private set; get; }

    private DemolishType _demolishType = DemolishType.None;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    private Vector2 _rectRenderStart;
    private Vector2 _rectRenderEnd;

    //begin to receive event of RectRender
    private void StartObserveRectRender()
    {
        if (_demolishType == DemolishType.None)
        {
            return;
        }

        TouchEventHandler evenntHandler = Camera.main.GetComponent<TouchEventHandler>();
        evenntHandler.isWorking = true;
        evenntHandler.OnTouchBegin += (object o, TouchEventHandler.TouchEventArgs e) =>
        {
            _rectRenderStart = e.position;
        };

        evenntHandler.OnTouchEnd += (object o, TouchEventHandler.TouchEventArgs e) =>
        {
            _rectRenderEnd = e.position;
            CheckObjectsInRect();
        };


    }

    private void CheckObjectsInRect()
    {
        LineRender render = Camera.main.GetComponent<LineRender>();
        render.drawArraw(new Vector2(0, 0), new Vector2(300, 300), Color.red);

        //start, end position both are world point
        Collider2D[] colliders = Physics2D.OverlapAreaAll(_rectRenderStart, _rectRenderEnd);


        if (_demolishType == DemolishType.CutTree)
        {
            List<Task> cutTreeTasks = new List<Task>();
            foreach (Collider2D coll in colliders)
            {
                CutTrees cutTrees = coll.GetComponent<CutTrees>();
                if (cutTrees != null)
                {
                    cutTrees.SetIsCutting(true);
                    cutTreeTasks.Add(new Task() { 
                        workTransform = coll.transform, 
                        typeSO = coll.gameObject.GetComponent<ResourceTypeHolder>().GetResourceTypeSO() 
                    });
                }
            }
            WorkingManager.Instance.AddWorks(cutTreeTasks);
        }
        else if (_demolishType == DemolishType.Digging)
        {
            CheckCollidersWhenDigging(colliders);
        }
        else if (_demolishType == DemolishType.PullDown)
        {
            CheckCollidersWhenPullDown(colliders);
        }
    }


    //todo: it is better not use building name.
    public void SetDemolishType(BuildingTypeSO typeSO)
    {
        DrawGrid drawGrid = Camera.main.GetComponent<DrawGrid>();
        drawGrid.isShowGrid = false;

        if (typeSO == null)
        {
            _demolishType = DemolishType.None;
        }
        else
        {
            switch (typeSO.buildingName)
            {
                case "Digging":

                    drawGrid.isShowGrid = true;

                    _demolishType = DemolishType.Digging;
                    break;
                case "CutTree":
                    _demolishType = DemolishType.CutTree;
                    break;
                case "PullDownBuilding":
                    _demolishType = DemolishType.PullDown;
                    break;
            }
        }

        StartObserveRectRender();
    }

    //==========================================================================
    //==========================================================================

    private void CheckCollidersWhenDigging(Collider2D[] colliders) 
    {
        foreach (Collider2D col in colliders)
        {
            Vector3 abovePosition = new Vector3(col.transform.position.x, col.transform.position.y + 1, 0);
            GameObject aboveObj = UtilsClass.GetObjectByRay(abovePosition);
            if (aboveObj != null && aboveObj.tag != "Soil")
            {

                //if above has object && object.buildOnSoil == true ==> can not destory
                BuildingTypeSOHolder holder = aboveObj.GetComponent<BuildingTypeSOHolder>();
                if (holder != null && holder.buidlingTypeSO.buildOnSoil)
                {
                    continue;
                }
            }

            //if above is soil too.
            if (col != null && col.gameObject != null && col.gameObject.tag == "Soil")
            {
                Destroy(col.gameObject);
            }
        }
    }

    //target must be building/road.
    private void CheckCollidersWhenPullDown(Collider2D[] colliders)
    {
        List<GameObject> toPullDownObjects = new List<GameObject>();
        foreach (Collider2D coll in colliders)
        {
            BuildingTypeSOHolder holder = coll.GetComponent<BuildingTypeSOHolder>();
            if (holder != null && holder.buidlingTypeSO != null)
            {
                BuildingType type = holder.buidlingTypeSO.type;
                if (type == BuildingType.Building || type == BuildingType.Road)
                {
                    Destroy(coll.gameObject);
                }
            }
        }
    }



}
