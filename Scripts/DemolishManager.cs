using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DemolishType
{
    None,
    Digging,
    CutTree,
    PullDownBuilding,
    Harvest,
    Collect,
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

        RectRender rectRender = Camera.main.GetComponent<RectRender>();
        rectRender.OnDrawRectStartPosition += (object o, RectRender.RectRenderEventHandlerArgs e) =>
        {
            _rectRenderStart = e.position;
        };
        
        rectRender.OnDrawRectEndPosition += (object o, RectRender.RectRenderEventHandlerArgs e) =>
        {
            _rectRenderEnd = e.position;
            ChechSelectedObjects();
        };
    }

    private void ChechSelectedObjects()
    {
        //Debug.Log("ChechSelectedObjects");

        //start, end position both are world point
        Collider2D[] colliders = Physics2D.OverlapAreaAll(_rectRenderStart, _rectRenderEnd);

        //Debug.Log("draw render colliders " + colliders.Length.ToString());

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
            foreach (Collider2D col in colliders)
            {
                Vector3 abovePosition = new Vector3(col.transform.position.x, col.transform.position.y + 1, 0);
                GameObject aboveObj = UtilsClass.GetObjectByRay(abovePosition);
                if (aboveObj != null && aboveObj.tag != "Soil")
                {
                    BuildingTypeSOHolder holder = aboveObj.GetComponent<BuildingTypeSOHolder>();
                    if (holder != null && holder.buidlingTypeSO.buildOnSoil)
                    {
                        continue;
                    }
                }

                if (col != null && col.gameObject != null && col.gameObject.tag == "Soil")
                {
                    Destroy(col.gameObject);
                }
            }

        }
    }

    public void SetDemolishType(BuildingTypeSO typeSO)
    {
        if (typeSO == null)
        {
            _demolishType = DemolishType.None;
        }
        else
        {
            switch (typeSO.buildingName)
            {
                case "Digging":
                    _demolishType = DemolishType.Digging;
                    break;
                case "CutTree":
                    _demolishType = DemolishType.CutTree;
                    break;

            }
        }

        StartObserveRectRender();
    }
}
