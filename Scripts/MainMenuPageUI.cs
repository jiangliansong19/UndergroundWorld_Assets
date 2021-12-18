using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameMode
{
    NewGame,
    CreateMap,
    LoadGame,
}



public class MainMenuPageUI : MonoBehaviour
{
    private Button _continueButton;
    private Button _newGameButton;
    private Button _loadGameButton;
    private Button _createMapButton;
    private Button _exitGameButton;



    private Scene currentScene;

    public enum Scene
    {
        GameScene,
        MainMenuScene,
    }



    // Start is called before the first frame update
    void Start()
    {
        Transform contentTransform = transform.Find("Content");
        _continueButton = contentTransform.Find("ContinueButton").GetComponent<Button>();
        _newGameButton = contentTransform.Find("NewGameButton").GetComponent<Button>();
        _loadGameButton = contentTransform.Find("LoadGameButton").GetComponent<Button>();
        _createMapButton = contentTransform.Find("CreateMapButton").GetComponent<Button>();
        _exitGameButton = contentTransform.Find("ExitGameButton").GetComponent<Button>();


        _continueButton.onClick.AddListener(OnClickContinuePlayGameButton);
        _newGameButton.onClick.AddListener(OnClickOnNewGameButton);
        _loadGameButton.onClick.AddListener(OnClickLoadGameButton);
        _createMapButton.onClick.AddListener(OnClickCreateMapButton);
        _exitGameButton.onClick.AddListener(OnClickExitGameButton);

    }


    private void OnClickContinuePlayGameButton()
    {
        PlayerPrefs.SetString("GameMode", GameMode.LoadGame.ToString());
        SceneManager.LoadSceneAsync(Scene.GameScene.ToString());
    }

    private void OnClickOnNewGameButton()
    {
        Debug.Log("Load game scene");

        PlayerPrefs.SetString("GameMode", GameMode.NewGame.ToString());
        SceneManager.LoadSceneAsync(Scene.GameScene.ToString());
    }

    private void OnClickLoadGameButton()
    {
        PlayerPrefs.SetString("GameMode", GameMode.LoadGame.ToString() + "x");
        SceneManager.LoadSceneAsync(Scene.GameScene.ToString());
    }

    private void OnClickCreateMapButton()
    {

    }

    private void OnClickExitGameButton()
    {

    }


    public void SetCurrentScene(Scene s)
    {
        currentScene = s;
    }

    public Scene GetCurrentScene()
    {
        return currentScene;
    }
}
