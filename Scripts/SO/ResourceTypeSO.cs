using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    Worker,
    Food,
    Water,
    Wood,
    Stone,
    Iron,
    Steel,
    RareMetal,
    Weapon,
}

[System.Serializable]
public class ResourceTypeAmount
{
    public ResourceTypeSO resourceType;
    public ulong amount;
}


[CreateAssetMenu(fileName = "ResourceTypeSO")]
public class ResourceTypeSO : ScriptableObject
{
    public string nameString;
    public Sprite sprite;
    public ResourceType type;


    public float collectSpendTime;//�ռ�����ʱ��
    public float amount;//����
}
