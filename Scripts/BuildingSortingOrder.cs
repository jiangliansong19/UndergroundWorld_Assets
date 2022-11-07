using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSortingOrder : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //y值与sortingOrder成反比。y值越小，图层越在表面。
        GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;
    }
}
