using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveData
{

}

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { private set; get; }

    public List<BuildingRunData> buildingDatas;
    public List<ScienceNodeSO> scienceNodes;
    public List<ResourceTypeAmount> resourceTypeAmounts;
    public List<ExploreTeam> exploreTeams;
    

    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveByBin(string filePath)
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(filePath);
        bf.Serialize(fs, new SaveData());
        fs.Close();

        if (File.Exists(filePath))
        {
            Debug.Log("save success");
        }
    }

    public void LoadByBin(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Open(filePath, FileMode.Open);
        SaveData data = (SaveData)bf.Deserialize(fileStream);
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
        new DialogUI().ShowInputDialog("ReName", (string name) =>
        {



            Debug.Log("input text: " + name);


            return "";
        }, () =>
        {


            return 0;
        });
    }
}
