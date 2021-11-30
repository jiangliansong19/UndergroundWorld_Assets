using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance { private set; get; }
    public event EventHandler OnResourcesChangedEvent;


    private Dictionary<ResourceTypeSO, ulong> resourcesDictionary;
    private ResourceTypeListSO resourceTypeListSO;

    private float updateResourcesMaxTime = 1f;
    private float updateResourcesTime;

    private void Awake()
    {
        Instance = this;

        resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        //������Դ����
        //���ȼ��ر��ش浵���ݣ����û�У�����ؿ������ݡ�
        resourcesDictionary = new Dictionary<ResourceTypeSO, ulong>();
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Food)] = 100;
        resourcesDictionary[resourceTypeListSO.GetResourceTypeSO(ResourceType.Water)] = 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnResourcesChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        updateResourcesTime -= Time.deltaTime;
        if (updateResourcesTime <= 0)
        {
            updateResourcesTime += updateResourcesMaxTime;
            UpdateResourceAmountImediately();
        }
    }

    private void UpdateResourceAmountImediately()
    {
        OnResourcesChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public Dictionary<ResourceTypeSO, ulong> GetResourcesDictionary()
    {
        return this.resourcesDictionary;
    }

    public void AddResource(ResourceTypeSO resourceTypeSO, ulong amount)
    {
        this.resourcesDictionary[resourceTypeSO] += amount;
        UpdateResourceAmountImediately();
    }

    public void SpendResource(ResourceTypeSO resourceTypeSO, ulong amount)
    {
        this.resourcesDictionary[resourceTypeSO] -= amount;
        UpdateResourceAmountImediately();
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
        return this.resourcesDictionary[resourceTypeSO] >= amount;
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
