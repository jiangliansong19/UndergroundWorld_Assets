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


    /// <summary>
    /// 立即调用事件，通告资源变化
    /// </summary>
    /// <param name="typeSO">资源类型</param>
    /// <param name="amount">资源数量</param>
    private void NoticeChangeOfResourceAmountImediately(ResourceTypeSO typeSO, long amount)
    {
        ResourceTypeAmount typeAmount = new ResourceTypeAmount() { resourceType = typeSO, amount = amount };
        OnResourcesChangedEvent?.Invoke(this, new ResourceChangeAmountArgs() { typeAmount = typeAmount });
    }


    /// <summary>
    /// 增加资源    
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <param name="amount">资源数量</param>
    public void AddResource(ResourceType resourceType, long amount)
    {
        if (amount == 0)
        {
            return;
        }

        ResourceTypeSO resourceTypeSO = resourceTypeListSO.GetResourceTypeSO(resourceType);
        resourcesDictionary[resourceTypeSO] += (long)amount;
        NoticeChangeOfResourceAmountImediately(resourceTypeSO, resourcesDictionary[resourceTypeSO]);
    }

    /// <summary>
    /// 是否可以支付资源？
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <param name="amount">资源数量</param>
    /// <returns>是否能支付？</returns>
    public bool CanAffordResource(ResourceType resourceType, long amount)
    {
        return resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(resourceType)] >= amount;
    }

    /// <summary>
    /// 获取资源数量
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <returns>资源数量</returns>
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

    /// <summary>
    /// 获取所有资源的map
    /// </summary>
    /// <returns></returns>
    public Dictionary<ResourceTypeSO, long> GetResourcesDictionary()
    {
        return resourcesDictionary;
    }

    /// <summary>
    /// 是否每个循环周期，都增加资源。；例如某建筑每周期产生100木头。
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <param name="amount">资源数量</param>
    /// <param name="everyCycle">是否按循环周期增加资源</param>
    public void AddResourcePerCycle(ResourceType resourceType, long amount, bool everyCycle = true)
    {
        ResourceTypeSO resourceTypeSO = resourceTypeListSO.GetResourceTypeSO(resourceType);
        if (everyCycle)
        {
            _resourceAmountsPerCycleDict[resourceTypeSO] += amount;
        }
        else
        {
            AddResource(resourceType, amount);
        }
    }

    /// <summary>
    /// 重置某资源的数量
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <param name="amount">资源数量</param>
    public void ResetResourceAmount(ResourceType resourceType, long amount)
    {
        ResourceTypeSO resourceTypeSO = resourceTypeListSO.GetResourceTypeSO(resourceType);
        resourcesDictionary[resourceTypeSO] = amount;
        NoticeChangeOfResourceAmountImediately(resourceTypeSO, resourcesDictionary[resourceTypeSO]);
    }
}
