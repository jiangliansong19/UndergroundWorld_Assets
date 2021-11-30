using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObject/ArchitectureListSO")]
public class ArchitectureListSO : ScriptableObject
{
    public List<ArchitectureSO> list;
}
