using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public static DialogUI Instance { private set; get; }

    private Image backgroundImage;
    private Text titleText;
    private Text messageText;
    private Button okButton;
    private Button cancelButton;
    private Transform dialog;

    private Func<int> selfOkClick;
    private Func<int> selfCancelClick;

    private void Awake()
    {
        Instance = this;

        dialog = transform.Find("Dialog");

        backgroundImage = dialog.Find("Image").GetComponent<Image>();
        titleText = dialog.Find("Title").GetComponent<Text>();
        messageText = dialog.Find("Content").GetComponent<Text>();
        okButton = dialog.Find("OKButton").GetComponent<Button>();
        cancelButton = dialog.Find("CancelButton").GetComponent<Button>();


        okButton.onClick.AddListener(OnClickOnOkButton);
        cancelButton.onClick.AddListener(OnClickOnCancelButton);

        transform.gameObject.SetActive(false);
    }

    private void OnClickOnCancelButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
        selfCancelClick();
        transform.gameObject.SetActive(false);
    }

    private void OnClickOnOkButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
        selfOkClick();
        transform.gameObject.SetActive(false);
    }

    public void ShowDialog(string title, string message, Func<int> okClick, Func<int> cancelClick)
    {
        transform.gameObject.SetActive(true);

        titleText.text = title;
        messageText.text = message;

        selfOkClick = okClick;
        selfCancelClick = cancelClick;
    }
}
