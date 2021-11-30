using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfoDialog : MonoBehaviour
{
    [SerializeField] private Transform dialog;
    private Transform content;

    // Start is called before the first frame updat

    public void ShowInfoDialog(GameObject building, Vector3 position)
    {
        Transform obj = Instantiate(dialog, position, Quaternion.identity);
        BuildingRunData data = building.GetComponent<BuildingRunData>();
        obj.Find("Content").GetComponent<TMPro.TextMeshPro>().SetText(data.incomePerDay.ToString());
    }
}
