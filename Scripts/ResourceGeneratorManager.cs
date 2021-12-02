using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 资源生产者。
/// </summary>
public class ResourceGeneratorManager : MonoBehaviour
{

    public static ResourceGeneratorManager Instance { private set; get; }
    List<ResourceTypeAmount> resourceAmountsPerCycle;//一个周期内，各类生产的资源总数。


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
