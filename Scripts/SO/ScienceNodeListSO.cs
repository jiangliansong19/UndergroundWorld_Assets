using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScienceNodeListSO")]
public class ScienceNodeListSO : ScriptableObject
{
    public ScienceCategoryType categoryType;
    public List<ScienceNodeSO> list;
}
