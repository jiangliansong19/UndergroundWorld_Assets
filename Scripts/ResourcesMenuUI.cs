using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// show resources number
/// </summary>
public class ResourcesMenuUI : MonoBehaviour
{
    [SerializeField] private RectTransform parantCanvas;

    private ResourceTypeListSO resourceTypeListSO;
    private Dictionary<ResourceTypeSO, Transform> resourceTypeTransformDict;

    private Transform templateTransform;
    private float itemHeight = 20f;


    private void Awake()
    {
        templateTransform = transform.Find("ResourcesMenuItem");
        templateTransform.gameObject.SetActive(false);

        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        resourceTypeTransformDict = new Dictionary<ResourceTypeSO, Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<ResourceTypeSO, long> resourceDict = ResourcesManager.Instance.GetResourcesDictionary();

        int i = 0;
        foreach (ResourceTypeSO item in resourceTypeListSO.list)
        {
            Transform aTransform = Instantiate(templateTransform, transform);
            aTransform.gameObject.SetActive(true);
            aTransform.Find("Icon").GetComponent<Image>().sprite = item.sprite;
            long resourceAmount = 0;
            if (resourceDict.Keys.Contains(item))
            {
                resourceAmount = resourceDict[item];
            }
            aTransform.Find("Amount").GetComponent<TextMeshProUGUI>().text = resourceAmount.ToString();

            aTransform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -itemHeight * i, 0);

            resourceTypeTransformDict[item] = aTransform;

            i++;
        }

        ResourcesManager.Instance.OnResourcesChangedEvent += ResourceManager_OnResourcesChangedEvent;
    }

    private void ResourceManager_OnResourcesChangedEvent(object sender, ResourcesManager.ResourceChangeAmountArgs e)
    {
        UpdateAmountNumber(e.typeAmount);
    }

    private void UpdateAmountNumber(ResourceTypeAmount typeAmount)
    {
        Transform targetTransform = resourceTypeTransformDict[typeAmount.resourceType];
        targetTransform.Find("Amount").GetComponent<TextMeshProUGUI>().text = typeAmount.amount.ToString();
    }

}
