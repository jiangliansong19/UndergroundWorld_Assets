using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPageUI : MonoBehaviour
{
    private Button newGameButton;



    public enum Scene
    {
        GameScene,
        MainMenuScene,
    }


    // Start is called before the first frame update
    void Start()
    {
        newGameButton = transform.Find("NewGameButton").GetComponent<Button>();
        newGameButton.onClick.AddListener(OnClickOnNewGameButton);
    }


    private void OnClickOnNewGameButton()
    {
        Debug.Log("Load game scene");
        SceneManager.LoadSceneAsync(Scene.GameScene.ToString());
    }
}
