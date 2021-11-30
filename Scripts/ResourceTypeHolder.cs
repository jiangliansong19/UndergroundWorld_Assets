using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTypeHolder : MonoBehaviour
{
    [SerializeField] private ResourceTypeSO resourceTypeSO;

    public ResourceTypeSO GetResourceTypeSO()
    {
        return resourceTypeSO;
    }
}
