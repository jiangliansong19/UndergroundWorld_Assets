using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private Task target;

    private float moveSpeed = 1.0f;

    private float workSpendTimer;
    private float workSpendTimeMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && target.workTransform != null && target.workTransform.gameObject != null)
        {

            if (Mathf.Abs(target.workTransform.position.x - transform.position.x) > 0.3)
            {
                //walking animation
                //todo

                //move position
                transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * moveSpeed;
                transform.position = new Vector3(transform.position.x, -0.3f, 0);
            }
            else
            {
                //idle animation

                //do work
                workSpendTimer -= Time.deltaTime;
                if (workSpendTimer <= 0)
                {
                    workSpendTimer += workSpendTimeMax;
                    ResourcesManager.Instance.AddResource(new ResourceTypeAmount() { resourceType = target.typeSO, amount = (ulong)target.typeSO.amount });

                    Debug.LogFormat("add {0} {1}", target.typeSO.nameString, target.typeSO.amount);

                    WorkingManager.Instance.RemoveWork(target);
                    Destroy(target.workTransform.gameObject);
                    target = null;
                }

            }
        }
    }

    public void setTarget(Task t)
    {

        workSpendTimeMax = t.typeSO.collectSpendTime;
        workSpendTimer = workSpendTimeMax;
        target = t;


    }

    public Task GetTarget()
    {
        return target;
    }
}
