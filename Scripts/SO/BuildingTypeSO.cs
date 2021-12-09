using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType
{

    House, 
    Road,
    DigHole,
    other,
}

public enum BuildingComponentType
{
    Basis, //基础
    Upgrade, //升级
    Component, //组件
    Strengthen, //强化
}



[CreateAssetMenu(fileName = "ScriptableObject/Buildings/BuildingTypeSO")]
public class BuildingTypeSO : ScriptableObject
{
    public BuildingType type;//类型
    public string buildingName;//建筑名
    public Sprite sprite;
    public Transform prefab;//UI模型

    public ResourceTypeAmount generateResource; //生产资源
    public List<ResourceTypeAmount> constructionCosts; //建筑消耗资源


    public int workersNumber;//工作人数



    public BuildingComponentType componentType; //组成部分


    public List<BuildingTypeSO> upgrades;//升级建筑
    public List<BuildingTypeSO> components;//组件建筑
    public List<BuildingTypeSO> strengthens;//强化建筑



    public bool continuousBuild;//框选后范围建造
    public bool buildOnSoil;//只能建在土壤(方格)上。



    public string remark;//备注，注释，方便自己看

    public string GetBuildingDescription()
    {
        string desc = "建筑建造信息";

        //desc += "Construction Cost " + UtilsClass.GetStringWithColor(data.constructCost.ToString(), "#FF0000");

        //if (data.originIncomePerDay > 0)
        //{
        //    desc += " Income/Day " + UtilsClass.GetStringWithColor(data.originIncomePerDay.ToString(), "#00FF00");
        //}

        return desc;
    }
}
