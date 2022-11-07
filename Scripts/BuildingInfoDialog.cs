using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ��ʾ������Ϣ
/// </summary>
public class BuildingInfoDialog : MonoBehaviour
{
    public static BuildingInfoDialog Instance { private set; get; }

    [SerializeField] private Text _titleText;
    [SerializeField] private Text _infoText;

    [SerializeField] private Button _demolishButton;//�ݻ�
    [SerializeField] private Button _upgradeButton;//����
    [SerializeField] private Button _closeButton;//�رյ���


    private Transform _buildingTransfrom;


    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        _demolishButton.onClick.AddListener(OnClickDemolishBuildingButton);
        _upgradeButton.onClick.AddListener(OnClickUpgradeBuildingButton);
        _closeButton.onClick.AddListener(OnClickCloseButton);
    }


    public void ShowBuildingInfo(Transform obj)
    {


        gameObject.SetActive(true);

        _buildingTransfrom = obj;

        BuildingTypeSOHolder holder = obj.GetComponent<BuildingTypeSOHolder>();

        if (holder != null)
        {
            BuildingTypeSO tyepSO = holder.buidlingTypeSO;
            _titleText.text = tyepSO.buildingName;
            _infoText.text = tyepSO.GetBuildingDescription();
        }

    }


    //�ݻٽ���
    private void OnClickDemolishBuildingButton()
    {
        Destroy(_buildingTransfrom.gameObject);
    }

    //��������
    private void OnClickUpgradeBuildingButton()
    {

    }

    //�رյ���
    private void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
}
