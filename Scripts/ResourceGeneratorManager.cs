using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ��Դ�����ߡ�
/// </summary>
public class ResourceGeneratorManager : MonoBehaviour
{

    public static ResourceGeneratorManager Instance { private set; get; }
    List<ResourceTypeAmount> resourceAmountsPerCycle;//һ�������ڣ�������������Դ������


    private float timer;
    private float timeCycleMax = 1f;


    private void Awake()
    {
        Instance = this;
        resourceAmountsPerCycle = new List<ResourceTypeAmount>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourcesPerCycle();
    }





    private void UpdateResourcesPerCycle()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timeCycleMax;

            ResourcesManager.Instance.AddResourceAmounts(resourceAmountsPerCycle);
        }
    }




    public void AddResourcePerCycle(ResourceTypeAmount amount) 
    {
        resourceAmountsPerCycle.Add(amount);
    }
}
