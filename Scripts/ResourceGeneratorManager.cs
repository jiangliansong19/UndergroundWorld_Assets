using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceGeneratorManager : MonoBehaviour
{

    public static ResourceGeneratorManager Instance { private set; get; }
    List<ResourceTypeAmount> resourceAmountsPerCycle;


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




    public void AddResourcePerCycle(ResourceTypeAmount amount, bool everyCycle = true) 
    {
        if (everyCycle)
        {
            resourceAmountsPerCycle.Add(amount);
        }
        else
        {
            ResourcesManager.Instance.AddResource(amount);
        }
    }
}
