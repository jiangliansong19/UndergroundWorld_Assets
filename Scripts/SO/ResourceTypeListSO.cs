using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceTypeListSO")]
public class ResourceTypeListSO : ScriptableObject
{
    
    public List<ResourceTypeSO> list;

    public ResourceTypeSO GetResourceTypeSO(ResourceType type)
    {
        foreach (ResourceTypeSO item in list)
        {
            if (item.type == type)
            {
                return item;
            }
        }
        return null;
    }
}
