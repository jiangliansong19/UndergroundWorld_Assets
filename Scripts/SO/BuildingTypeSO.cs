using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType
{

    House,
    Road,
    DigHole,

}

public enum BuildingFunctionType
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

    public ResourceTypeAmount generateResource;
    public List<ResourceTypeAmount> constructionCosts;

    public BuildingFunctionType functionType;

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
