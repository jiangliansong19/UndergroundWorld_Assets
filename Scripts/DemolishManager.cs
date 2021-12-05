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
    private Vector3? longPressOrigin;
    private DemolishType _demolishType;

    private RectRender _rectRender;
    private Vector2 _rectRenderStart;
    private Vector2 _rectRenderEnd;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rectRender = Camera.main.GetComponent<RectRender>();
        _rectRender.OnDrawRectEndPosition += RectRender_OnDrawRectEndPosition;
        _rectRender.OnDrawRectStartPosition += RectRender_OnDrawRectStartPosition;
    }

    private void RectRender_OnDrawRectStartPosition(object sender, RectRender.RectRenderEventHandlerArgs e)
    {
        _rectRenderStart = e.position;
    }

    private void RectRender_OnDrawRectEndPosition(object sender, RectRender.RectRenderEventHandlerArgs e)
    {
        _rectRenderEnd = e.position;

        ChechSelectedObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (_demolishType != DemolishType.None)
        {

        }

        if (_demolishType == DemolishType.Digging) 
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("GetMouseButtonDown");
                longPressOrigin = UtilsClass.GetCurrentWorldPoint();
            }
            if (Input.GetMouseButton(0))
            {

            }
            if (Input.GetMouseButtonUp(0) && longPressOrigin != null && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("GetMouseButtonUp");
                if (this._demolishType == DemolishType.Digging)
                {
                    Digging();
                }

                longPressOrigin = null;
            }
        }

    }










    private void ChechSelectedObjects()
    {
        Debug.Log("ChechSelectedObjects");

        Collider2D[] colliders = Physics2D.OverlapAreaAll(_rectRenderStart, _rectRenderEnd);
        Collider2D[] collider2s = Physics2D.OverlapCircleAll(new Vector2(0, 0), 40);

        Debug.Log("draw render colliders " + colliders.Length.ToString());
        Debug.Log("draw render collider2s " + collider2s.Length.ToString());

        if (_demolishType == DemolishType.CutTree)
        {
            foreach (Collider2D coll in colliders)
            {
                CutTrees cutTrees = coll.GetComponent<CutTrees>();
                if (cutTrees != null)
                {
                    cutTrees.SetIsCutting(true);
                }
            }
        }

    }







    private void Digging()
    {
        Vector3 start = (Vector3)longPressOrigin;
        TryToDigging(start);

        Vector3 end = UtilsClass.GetCurrentWorldPoint();
        for (float i = Mathf.Min(start.x, end.x); i <= Mathf.Max(start.x, end.x); i++)
        {
            for (float j = Mathf.Min(start.y, end.y); j <= Mathf.Max(start.y, end.y); j++)
            {
                TryToDigging(new Vector3(i, j, 0));
            }
        }
    }

    private void TryToDigging(Vector3 position)
    {
        //above is not null && is not soil
        GameObject aboveObj = UtilsClass.GetObjectByRay(new Vector3(position.x, position.y + 1, 0));
        if (aboveObj != null && aboveObj.tag != "Soil")
        {
            return;
        }

        GameObject currentObj = UtilsClass.GetObjectByRay(position);
        if (currentObj != null && currentObj.tag == "Soil")
        {
            Destroy(currentObj);
            return;
        }
    }



    public void SetDemolishType(BuildingTypeSO typeSO)
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
}
