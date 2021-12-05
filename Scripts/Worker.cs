using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private Working target;

    private float moveSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {

            if (Mathf.Abs(target.workTransform.position.x - transform.position.x) > 0.2)
            {
                transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * moveSpeed;
            } else
            {


            }
        }
    }

    public void setTarget(Working t)
    {
        target = t;
    }

    public Working GetTarget()
    {
        return target;
    }
}
