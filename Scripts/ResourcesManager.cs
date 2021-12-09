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
    private Dictionary<ResourceTypeSO, long> resourcesDictionary;
    private Dictionary<ResourceTypeSO, long> _resourceAmountsPerCycleDict;


    private ResourceTypeListSO resourceTypeListSO;


    private float timer;
    private float timeCycleMax = 1f;

    private void Awake()
    {
        Instance = this;

        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourcesDictionary = new Dictionary<ResourceTypeSO, long>();
        _resourceAmountsPerCycleDict = new Dictionary<ResourceTypeSO, long>();


        foreach (ResourceTypeSO typeSO in resourceTypeListSO.list)
        {
            resourcesDictionary[typeSO] = 0;
            _resourceAmountsPerCycleDict[typeSO] = 0;
        }
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Food)] = 100;
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Water)] = 100;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        UpdateResourcesPerCycle();
    }



    private void UpdateResourcesPerCycle()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timeCycleMax;

            foreach (ResourceTypeSO typeSO in _resourceAmountsPerCycleDict.Keys)
            {
                AddResource(typeSO.type, _resourceAmountsPerCycleDict[typeSO]);
            }
        }
    }



    private void NoticeChangeOfResourceAmountImediately(ResourceTypeSO typeSO, long amount)
    {
        ResourceTypeAmount typeAmount = new ResourceTypeAmount() { resourceType = typeSO, amount = amount };
        OnResourcesChangedEvent?.Invoke(this, new ResourceChangeAmountArgs() { typeAmount = typeAmount });
    }



    public void AddResource(ResourceType resourceType, long amount)
    {
        if (amount == 0)
        {
            return;
        }

        ResourceTypeSO resourceTypeSO = GetResourtypeSO(resourceType);
        resourcesDictionary[resourceTypeSO] += (long)amount;
        NoticeChangeOfResourceAmountImediately(resourceTypeSO, resourcesDictionary[resourceTypeSO]);
    }


    public bool CanAffordResource(ResourceType resourceType, long amount)
    {
        return resourcesDictionary[GetResourtypeSO(resourceType)] >= amount;
    }


    public long GetResourceAmount(ResourceType resourceType)
    {
        foreach (ResourceTypeSO resourceTypeSO in resourcesDictionary.Keys)
        {
            if (resourceTypeSO.type == resourceType)
            {
                return resourcesDictionary[resourceTypeSO];
            }
        }
        return 0;
    }



    public Dictionary<ResourceTypeSO, long> GetResourcesDictionary()
    {
        return resourcesDictionary;
    }



    public ResourceTypeSO GetResourtypeSO(ResourceType type)
    {
        return resourceTypeListSO.GetResourceTypeSO(type);
    }


    public void AddResourcePerCycle(ResourceType resourceType, long amount, bool everyCycle = true)
    {
        ResourceTypeSO resourceTypeSO = GetResourtypeSO(resourceType);
        if (everyCycle)
        {
            _resourceAmountsPerCycleDict[resourceTypeSO] += amount;
        }
        else
        {
            AddResource(resourceType, amount);
        }
    }
}
