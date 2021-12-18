using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPageUI : MonoBehaviour
{

    public static SettingPageUI Instance { private set; get; }

    private Button _cancelButton;
    private Button _saveGameButton;
    private Button _loadGameButton;
    private Button _exitToMainMenuButton;
    private Button _exitToDesktopButton;


    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        Transform contentTransform = transform.Find("SettingContent").Find("Content");
        _cancelButton = contentTransform.Find("CancelButton").GetComponent<Button>();
        _saveGameButton = contentTransform.Find("SaveGameButton").GetComponent<Button>();
        _loadGameButton = contentTransform.Find("LoadGameButton").GetComponent<Button>();
        _exitToMainMenuButton = contentTransform.Find("ExitToMainMenuButton").GetComponent<Button>();
        _exitToDesktopButton = contentTransform.Find("ExitToDesktopButton").GetComponent<Button>();


        _cancelButton.onClick.AddListener(OnClickCancelButton);
        _saveGameButton.onClick.AddListener(OnClickSaveGameButton);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnClickCancelButton()
    {
        gameObject.SetActive(false);
    }

    private void OnClickSaveGameButton()
    {
        Debug.Log("Click On save Button");
    }


}
