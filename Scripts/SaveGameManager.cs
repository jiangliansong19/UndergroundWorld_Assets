using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class SaveData
{
    public int scienceScore = 1000;

    //{"familyHouse" : 1.0, "energy" : 0.5 ....}
    public Dictionary<string, float> scienceNodeDict;
    public Dictionary<string, float> resourceDict;

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



    private string GetRootPath()
    {
        string result = GameProjectSettings.SaveDataRootPath;
        if (!Directory.Exists(result))
        {
            Directory.CreateDirectory(result);
        }
        return result;
    }

    public void SaveByBin(string fileName)
    {
        string targetFileName = GetRootPath() + fileName;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(targetFileName);
        bf.Serialize(fs, new SaveData());
        fs.Close();

        if (File.Exists(targetFileName))
        {
            Debug.Log("save success: " + targetFileName);
        }
    }

    public void LoadByBin(string fileName)
    {
        string targetFileName = GetRootPath() + fileName;
        if (!File.Exists(targetFileName))
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Open(targetFileName, FileMode.Open);
        SaveData data = (SaveData)bf.Deserialize(fileStream);
        fileStream.Close();

    }

    //==========================================================================
    //==========================================================================

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
