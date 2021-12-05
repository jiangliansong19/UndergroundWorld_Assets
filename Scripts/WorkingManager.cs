using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public class WorkingManager : MonoBehaviour
{

    public static WorkingManager Instance { private set; get; }

    private List<Transform> totalWorks;
    private List<Worker> totalWorkers;

    private float searchWorkTimer;
    private float searchWorkTimeMax = 1f;


    private void Awake()
    {
        Instance = this;
        totalWorks = new List<Transform>();
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
            if (worker.target == null)
            {

            }
        }
    }






    public void AddWorks(List<Transform> works)
    {
        foreach (var item in works)
        {
            totalWorks.Add(item);
        }
    }

    public void RemoveWork(Transform work)
    {
        totalWorks.Remove(work);
    }

    



    public void AddWorkers(List<Worker> workers)
    {
        foreach (var item in workers)
        {
            totalWorkers.Add(item);
        }
    }
}
