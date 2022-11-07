using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    private Image backgroundImage;
    private Text titleText;
    private Text messageText;
    private Text inputText;

    GameObject _inputField;
    GameObject _messageContent;

    private Button okButton;
    private Button cancelButton;
    private Transform dialog;


    private Func<int> selfOkClick;
    private Func<int> selfCancelClick;

    private Func<string, string> inputOkClick;


    public static DialogUI Create()
    {
        DialogUI dialogPrefab = Resources.Load<DialogUI>("pfDialogUI");
        DialogUI dialogUI = Instantiate(dialogPrefab, Vector2.zero, Quaternion.identity);
        return dialogUI;
    }

    private void Awake()
    {
        dialog = transform.Find("Dialog");

        _inputField = dialog.Find("InputField").gameObject;
        _messageContent = dialog.Find("Content").gameObject;

        backgroundImage = dialog.Find("Image").GetComponent<Image>();
        titleText = dialog.Find("Title").GetComponent<Text>();
        messageText = dialog.Find("Content").GetComponent<Text>();
        inputText = dialog.Find("InputField").GetComponent<Text>();
        okButton = dialog.Find("OKButton").GetComponent<Button>();
        cancelButton = dialog.Find("CancelButton").GetComponent<Button>();

        okButton.onClick.AddListener(OnClickOnOkButton);
        cancelButton.onClick.AddListener(OnClickOnCancelButton);
    }

    private void OnClickOnCancelButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
        selfCancelClick();

        Destroy(gameObject);
    }

    private void OnClickOnOkButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);

        if (selfOkClick != null)
        {
            _ = selfOkClick();
        }

        if (inputOkClick != null)
        {
            inputOkClick(_inputField.GetComponent<InputField>().text);
        }

        Destroy(gameObject);
    }

    public void ShowDialog(string title, string message, Func<int> okClick, Func<int> cancelClick)
    {
        _inputField.SetActive(false);
        _messageContent.SetActive(true);


        titleText.text = title;
        messageText.text = message;

        selfOkClick = okClick;
        selfCancelClick = cancelClick;
    }

    public void ShowInputDialog(string title, Func<string, string> okClick, Func<int> cancelClick)
    {
        _inputField.SetActive(true);
        _messageContent.SetActive(false);


        titleText.text = title;

        inputOkClick = okClick;
        selfCancelClick = cancelClick;
    }
}
