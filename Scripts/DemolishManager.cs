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
    private DemolishType demolishType;

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
        if (this.demolishType != DemolishType.None)
        {

        }
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
            if (this.demolishType == DemolishType.Digging)
            {
                Digging();
            }

            longPressOrigin = null;
        }
    }


    public void SetDemolishType(BuildingTypeSO typeSO)
    {
        switch(typeSO.buildingName)
        {
            case "Digging":
                this.demolishType = DemolishType.Digging;
                break;

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

}
