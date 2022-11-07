using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    //分数
    public int scienceScore = 1000;

    //{"familyHouse" : 1.0, "energy" : 0.5 ....}
    //科技
    public Dictionary<string, float> scienceNodeDict;

    //资源
    public Dictionary<string, long> resourceDict;

}

public class SaveGameManager: MonoBehaviour
{
    public static SaveGameManager Instance { private set; get; }

    //public List<BuildingRunData> buildingDatas;
    //public List<ScienceNodeSO> scienceNodes;
    //public List<ResourceTypeAmount> resourceTypeAmounts;
    //public List<ExploreTeam> exploreTeams;
    private void Awake()
    {
        Instance = this;





    }

    private SaveData CreateSaveData()
    {
        SaveData data = new SaveData();
        data.resourceDict = new Dictionary<string, long>();


        Dictionary<ResourceTypeSO, long> resourceDict = ResourcesManager.Instance.GetResourcesDictionary();
        foreach (var resourceTypeSO in resourceDict.Keys)
        {
            data.resourceDict.Add(resourceTypeSO.type.ToString(), resourceDict[resourceTypeSO]);
        }



        return data;
    }

    private void ReadSaveData(SaveData data)
    {
        Dictionary<ResourceTypeSO, long> resourceDict = ResourcesManager.Instance.GetResourcesDictionary();
        ResourceTypeListSO resourceTypeListSO = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (string resouceType in data.resourceDict.Keys)
        {
            Enum.TryParse(resouceType, out ResourceType type);
            ResourcesManager.Instance.ResetResourceAmount(type, data.resourceDict[resouceType]);
        }


    }


    private string GetRootPath()
    {
        string result = GameProjectSettings.SaveDataRootPath;
        if (!Directory.Exists(result))
        {
            Directory.CreateDirectory(result);
        }
        return result;
    }


    //==========================================================================
    //==========================================================================




    public void SaveByBin(string fileName)
    {
        string targetFileName = GetRootPath() + fileName;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(targetFileName);
        bf.Serialize(fs, CreateSaveData());
        fs.Close();

        if (File.Exists(targetFileName))
        {
            Debug.Log("save success: " + targetFileName);
        }
    }

    public void LoadByBin(string fileName)
    {
        string targetFileName = string.Empty;
        if (!fileName.Contains(GetRootPath()))
        {
            targetFileName = GetRootPath() + fileName;
        } else
        {
            targetFileName = fileName;
        }

        if (!File.Exists(targetFileName))
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Open(targetFileName, FileMode.Open);
        SaveData data = (SaveData)bf.Deserialize(fileStream);
        ReadSaveData(data);
        fileStream.Close();

    }


    public void OnClickOnSaveButton()
    {

    }

    public void OnClickOnOverrideButton()
    {



    }

    public void OnClickOnSaveNewFileButton()
    {
        DialogUI.Create().ShowInputDialog("ReName", (string name) =>
        {



            Debug.Log("input text: " + name);


            return "";
        }, () =>
        {


            return 0;
        });
    }
}