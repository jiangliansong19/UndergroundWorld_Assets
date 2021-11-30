using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//建筑类型：房屋，路灯
[CreateAssetMenu(fileName = "ScriptableObject/Architecture")]
public class ArchitectureSO : ScriptableObject
{
    public string typeName;
    public Transform prefab;
    public BuildingTypeListSO list;
}
