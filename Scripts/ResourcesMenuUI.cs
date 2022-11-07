using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 展示资源数量
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
        //icon模板
        templateTransform = transform.Find("ResourcesMenuItem");
        templateTransform.gameObject.SetActive(false);

        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        resourceTypeTransformDict = new Dictionary<ResourceTypeSO, Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        buildResourceUIItems();

        //建立通知观察者
        ResourcesManager.Instance.OnResourcesChangedEvent += ResourceManager_OnResourcesChangedEvent;
    }

    private void buildResourceUIItems()
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
    }

    /// <summary>
    /// 接收通知
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ResourceManager_OnResourcesChangedEvent(object sender, ResourcesManager.ResourceChangeAmountArgs e)
    {
        UpdateAmountNumber(e.typeAmount);
    }

    /// <summary>
    /// 接收通知
    /// </summary>
    /// <param name="typeAmount"></param>
    private void UpdateAmountNumber(ResourceTypeAmount typeAmount)
    {
        Transform targetTransform = resourceTypeTransformDict[typeAmount.resourceType];
        if (targetTransform != null)
        {
            targetTransform.Find("Amount").GetComponent<TextMeshProUGUI>().text = typeAmount.amount.ToString();
        }
    }

}
