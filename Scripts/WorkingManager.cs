using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Task
{
    public Transform workTransform;
    public ResourceTypeSO typeSO;
} 


public class WorkingManager : MonoBehaviour
{

    public static WorkingManager Instance { private set; get; }

    private List<Task> totalTasks;
    private List<Worker> totalWorkers;

    private float searchWorkTimer;
    private float searchWorkTimeMax = 1f;


    private void Awake()
    {
        Instance = this;
        totalTasks = new List<Task>();
        totalWorkers = new List<Worker>();
    }


    private void Update()
    {

        searchWorkTimer -= Time.deltaTime;
        if (searchWorkTimer <= 0)
        {
            searchWorkTimer += searchWorkTimeMax;


            executeWorks();
        }

    }






    private void executeWorks()
    {
        foreach (Worker worker in totalWorkers)
        {
            if (worker.GetTarget() == null && totalTasks.Count >= 1)
            {
                worker.setTarget(totalTasks[0]);
                Debug.LogFormat("Add task {0}", totalTasks[0].typeSO.nameString);
            }
        }
    }






    public void AddWorks(List<Task> works)
    {
        foreach (var item in works)
        {
            totalTasks.Add(item);
        }
    }

    public void RemoveWork(Task work)
    {
        totalTasks.Remove(work);
    }

    



    public void AddWorker(Worker w)
    {
        totalWorkers.Add(w);
        ResourcesManager.Instance.AddResource(new ResourceTypeAmount()
        {
            resourceType = w.GetComponent<ResourceTypeHolder>().GetResourceTypeSO(),
            amount = 1
        });
    }




}
