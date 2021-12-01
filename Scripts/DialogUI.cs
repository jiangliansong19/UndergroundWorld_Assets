using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    private Image backgroundImage;
    private Text titleText;
    private Text messageText;
    private Button okButton;
    private Button cancelButton;
    private Transform dialog;

    private Func<int> selfOkClick;
    private Func<int> selfCancelClick;


    public static DialogUI Create()
    {
        DialogUI dialogPrefab = Resources.Load<DialogUI>("pfDialogUI");
        DialogUI dialogUI = Instantiate(dialogPrefab, Vector2.zero, Quaternion.identity);
        return dialogUI;
    }

    private void Awake()
    {
        dialog = transform.Find("Dialog");

        backgroundImage = dialog.Find("Image").GetComponent<Image>();
        titleText = dialog.Find("Title").GetComponent<Text>();
        messageText = dialog.Find("Content").GetComponent<Text>();
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
        selfOkClick();

        Destroy(gameObject);
    }

    public void ShowDialog(string title, string message, Func<int> okClick, Func<int> cancelClick)
    {

        titleText.text = title;
        messageText.text = message;

        selfOkClick = okClick;
        selfCancelClick = cancelClick;
    }
}
