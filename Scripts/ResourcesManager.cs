using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 资源数量管理。只有增加资源，减少资源的方法。
/// </summary>
public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { private set; get; }

    //通告资源变化
    public event EventHandler<ResourceChangeAmountArgs> OnResourcesChangedEvent;
    public class ResourceChangeAmountArgs { public ResourceTypeAmount typeAmount; }


    //内部采用字典统计数据，加快资源数量的查找速度
    private Dictionary<ResourceTypeSO, ulong> resourcesDictionary;



    private ResourceTypeListSO resourceTypeListSO;


    private void Awake()
    {
        Instance = this;

        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        resourcesDictionary = new Dictionary<ResourceTypeSO, ulong>();
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Worker)] = 0;
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Food)] = 100;
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Water)] = 100;
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Wood)] = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void UpdateResourceAmountImediately(ResourceTypeSO typeSO, ulong amount)
    {
        ResourceTypeAmount typeAmount = new ResourceTypeAmount() { resourceType = typeSO, amount = amount };
        OnResourcesChangedEvent?.Invoke(this, new ResourceChangeAmountArgs() { typeAmount = typeAmount });
    }

    public Dictionary<ResourceTypeSO, ulong> GetResourcesDictionary()
    {
        return resourcesDictionary;
    }

    public void AddResource(ResourceTypeAmount typeAmount)
    {
        resourcesDictionary[typeAmount.resourceType] += typeAmount.amount;
        UpdateResourceAmountImediately(typeAmount.resourceType, resourcesDictionary[typeAmount.resourceType]);
    }

    public void AddResourceAmounts(List<ResourceTypeAmount> amounts)
    {
        foreach (ResourceTypeAmount amountType in amounts)
        {
            AddResource(amountType);
        }
    }

    public void SpendResource(ResourceTypeSO resourceTypeSO, ulong amount)
    {
        resourcesDictionary[resourceTypeSO] -= amount;
        UpdateResourceAmountImediately(resourceTypeSO, resourcesDictionary[resourceTypeSO]);
    }

    public void SpendResourceAmounts(List<ResourceTypeAmount> amounts)
    {
        foreach (ResourceTypeAmount amountType in amounts)
        {
            SpendResource(amountType.resourceType, amountType.amount);
        }
    }

    public bool CanAffordResource(ResourceTypeSO resourceTypeSO, ulong amount)
    {
        return resourcesDictionary[resourceTypeSO] >= amount;
    }

    public bool CanAffordResourceAmounts(List<ResourceTypeAmount> amounts)
    {
        foreach (ResourceTypeAmount amountType in amounts)
        {
            if (CanAffordResource(amountType.resourceType, amountType.amount) == false)
            {
                return false;
            }
        }
        return true;
    }
}
