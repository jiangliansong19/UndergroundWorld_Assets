using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingPageUI : MonoBehaviour
{

    public static SettingPageUI Instance { private set; get; }

    private Button _cancelButton;
    private Button _saveGameButton;
    private Button _loadGameButton;
    private Button _exitToMainMenuButton;
    private Button _exitToDesktopButton;

    private Transform _saveGameTransform;
    private Transform _savedGameListTransfrom;
    private Transform _itemTemplateTransform;
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
        _saveGameTransform = transform.Find("SaveGameContent");
        _saveGameTransform.gameObject.SetActive(false);
        _savedGameListTransfrom = _saveGameTransform.Find("ScrollView").Find("Viewport").Find("Content");
        _itemTemplateTransform = _savedGameListTransfrom.Find("TemplateItem");

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

        string[] files;
        if (!Directory.Exists(GameProjectSettings.SaveDataRootPath))
        {
            files = null;
        }
        files = Directory.GetFiles(GameProjectSettings.SaveDataRootPath);

        Button[] btns = new Button[files.Length];
        List<Button> buttons = new List<Button>();
        foreach (string file in files)
        {
            Transform recordTransform = Instantiate(_itemTemplateTransform, _savedGameListTransfrom);
            recordTransform.Find("Title").GetComponent<Text>().text = "Test123";
            recordTransform.Find("Content").GetComponent<Text>().text = "TestContent";

            buttons.Add(_itemTemplateTransform.GetComponent<Button>());

            _itemTemplateTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                _selectSaveFile = file;
            });

        }
        _itemTemplateTransform.GetComponent<Button>().interactable = false;




    }


    private void OnClickSaveGameButton()
    {
        

        DialogUI.Create().ShowInputDialog("Save Game", (string name) =>
        {

            Debug.Log("Click On save Button + " + name);

            SaveGameManager.Instance.SaveByBin(name);

            return "";
        }, () => { return 0; });

    }

    private void OnClickOnExitToMainButton()
    {

    }


    private void OnClickExitGameButton()
    {

    }


    private void OnClickSaveGameDeleteButton()
    {

    }

    private void OnClickSaveGameOverButton()
    {


        SaveGameManager.Instance.SaveByBin(_selectSaveFile);
    }

    private void OnClickSaveGameLoadButton()
    {
        
    }

    private void OnClickSaveGameCloseButton()
    {
        _saveGameTransform.gameObject.SetActive(false);
    }


    //==========================================================================
    //==========================================================================



    private void SetButtonSelect(Button btn, List<Button> others)
    {
        foreach (Button item in others)
        {
             
        }

        btn.onClick.AddListener(() =>
        {

        });
    }
}
