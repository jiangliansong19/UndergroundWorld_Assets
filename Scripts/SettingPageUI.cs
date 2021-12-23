using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine.SceneManagement;

public class SettingPageUI : MonoBehaviour
{

    public static SettingPageUI Instance { private set; get; }

    private Button _cancelButton;
    private Button _saveGameButton;
    private Button _loadGameButton;
    private Button _exitToMainMenuButton;
    private Button _exitToDesktopButton;

    [SerializeField] private Transform _saveGameTransform;
    [SerializeField] private Transform _savedGameListTransfrom;
    [SerializeField] private Transform _itemTemplateTransform;

    [SerializeField] private Button _saveGameDeleteButton;
    [SerializeField] private Button _saveGameOverButton;
    [SerializeField] private Button _saveGameLoadButton;
    [SerializeField] private Button _saveGameCloseButton;


    private string _selectSaveFile;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        Transform contentTransform = transform.Find("SettingContent").Find("Content");

        _saveGameTransform.gameObject.SetActive(false);

        _cancelButton = contentTransform.Find("CancelButton").GetComponent<Button>();
        _saveGameButton = contentTransform.Find("SaveGameButton").GetComponent<Button>();
        _loadGameButton = contentTransform.Find("LoadGameButton").GetComponent<Button>();
        _exitToMainMenuButton = contentTransform.Find("ExitToMainMenuButton").GetComponent<Button>();
        _exitToDesktopButton = contentTransform.Find("ExitToDesktopButton").GetComponent<Button>();
        


        _cancelButton.onClick.AddListener(OnClickCancelButton);
        _saveGameButton.onClick.AddListener(OnClickSaveGameButton);
        _loadGameButton.onClick.AddListener(OnClickLoadGameButton);
        _exitToMainMenuButton.onClick.AddListener(OnClickOnExitToMainButton);
        _exitToDesktopButton.onClick.AddListener(OnClickExitGameButton);


        _saveGameDeleteButton.onClick.AddListener(OnClickSaveGameDeleteButton);
        _saveGameOverButton.onClick.AddListener(OnClickSaveGameOverButton);
        _saveGameLoadButton.onClick.AddListener(OnClickSaveGameLoadButton);
        _saveGameCloseButton.onClick.AddListener(OnClickSaveGameCloseButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameProjectSettings.gameScene == GameScene.GameScene)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                gameObject.SetActive(true);
            }
        }
    }

    private void LoadLocalRecordsList()
    {
        string[] files;
        if (!Directory.Exists(GameProjectSettings.SaveDataRootPath))
        {
            files = null;
            return;
        }
        files = Directory.GetFiles(GameProjectSettings.SaveDataRootPath);


        for (int i = 1; i < _savedGameListTransfrom.childCount; i++)
        {
            Transform item = _savedGameListTransfrom.GetChild(i);
            Destroy(item.gameObject);
        }


        _itemTemplateTransform.GetComponent<Button>().interactable = true;
        foreach (string file in files)
        {
            if (!file.Contains("###"))
            {
                continue;
            }

            Transform recordTransform = Instantiate(_itemTemplateTransform, _savedGameListTransfrom);

            string fileName = System.IO.Path.GetFileName(file);
            string[] components = fileName.Split(new string[] { "###" }, System.StringSplitOptions.None);
            DateTime date = DateUtil.GetDateTime(components[0]);

            recordTransform.Find("Title").GetComponent<Text>().text = date.ToString("yyyy-MM-dd HH:mm:ss");
            recordTransform.Find("Content").GetComponent<Text>().text = components[1];

            recordTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                _selectSaveFile = file;
            });

        }
        _itemTemplateTransform.GetComponent<Button>().interactable = false;
    }

    //==========================================================================
    //==========================================================================


    private void OnClickCancelButton()
    {
        gameObject.SetActive(false);
    }


    private void OnClickLoadGameButton()
    {
        _saveGameTransform.gameObject.SetActive(true);
        LoadLocalRecordsList();
    }

    private void OnClickSaveGameButton()
    {
        

        DialogUI.Create().ShowInputDialog("Save Game", (string name) =>
        {
            int timeStamp = DateUtil.GetTimeStamp(System.DateTime.Now);
            SaveGameManager.Instance.SaveByBin(timeStamp.ToString() + "###" + name);

            Debug.Log("save data to local : " + timeStamp.ToString() + "###" + name);

            return "";
        }, () => { return 0; });

    }

    private void OnClickOnExitToMainButton()
    {
        DialogUI.Create().ShowDialog("Alert", "Game is not saved!",
        () => {

            SceneManager.LoadSceneAsync(GameScene.MainMenuScene.ToString());

            GameProjectSettings.gameScene = GameScene.MainMenuScene;

            return 0;
        },
        () => { return 0; });
    }

    private void OnClickExitGameButton()
    {
        DialogUI.Create().ShowDialog("Alert", "Game is not saved!",
            () => {
                Application.Quit();
                return 0; },
            () => { return 0; });
    }


    //delete a local record
    private void OnClickSaveGameDeleteButton()
    {
        Debug.Log("delete file : " + _selectSaveFile);

        File.Delete(_selectSaveFile);

        LoadLocalRecordsList();
    }

    //override a local record with new content
    private void OnClickSaveGameOverButton()
    {
        SaveGameManager.Instance.SaveByBin(_selectSaveFile);
    }

    //load a local record
    private void OnClickSaveGameLoadButton()
    {
        Debug.Log("load data : " + _selectSaveFile);

        SaveGameManager.Instance.LoadByBin(_selectSaveFile);

        _saveGameTransform.gameObject.SetActive(false);
    }

    //close save game panel
    private void OnClickSaveGameCloseButton()
    {
        _saveGameTransform.gameObject.SetActive(false);
    }

    //==========================================================================
    //==========================================================================

    public void ShowSettingPageUI()
    {
        gameObject.SetActive(true);

        Time.timeScale = 0;

        CameraHandler.Instance.SetHandleMouseEnable(false);

    }

    public void HideSettingPageUI()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1;

        CameraHandler.Instance.SetHandleMouseEnable(true);
    }
}
