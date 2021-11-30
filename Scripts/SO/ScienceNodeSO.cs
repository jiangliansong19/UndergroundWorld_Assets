using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ScienceCategoryType {

    Line,

    HouseBuilding,
    FoodAndDrink,
    Transport,
    Energy,

    Industry,

    ElectricSect,
    AnimalSect,

}



[CreateAssetMenu(fileName = "ScienceNodeSO")]
public class ScienceNodeSO : ScriptableObject
{

    public ScienceCategoryType nodeType;
    public string nodeDesc;
    public int nodeNumber; //节点顺序

    //前置节点(解锁节点)
    public List<ScienceNodeSO> previousNodes; 


    //解锁新建筑，
    //解锁建筑升级(不占位置，在原有建筑上升级)
    //解锁建筑功能（占位置，需要额外增加建筑空间）
    public List<BuildingTypeSO> unlockBuildingTypeSO;
    
}