using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProjectSettings
{

    //default is debug mode
    public static bool ISDEBUG = true;

    //game save data root
    public static string SaveDataRootPath = Application.dataPath.Replace("Assets", "") + "SavedGameData/";


    public static GameScene gameScene = GameScene.MainMenuScene;
}
