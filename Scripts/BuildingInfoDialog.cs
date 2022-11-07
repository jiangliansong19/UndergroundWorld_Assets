using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 显示建筑信息
/// </summary>
public class BuildingInfoDialog : MonoBehaviour
{
    public static BuildingInfoDialog Instance { private set; get; }

    [SerializeField] private Text _titleText;
    [SerializeField] private Text _infoText;

    [SerializeField] private Button _demolishButton;//摧毁
    [SerializeField] private Button _upgradeButton;//升级
    [SerializeField] private Button _closeButton;//关闭弹窗


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


    //摧毁建筑
    private void OnClickDemolishBuildingButton()
    {
        Destroy(_buildingTransfrom.gameObject);
    }

    //升级建筑
    private void OnClickUpgradeBuildingButton()
    {

    }

    //关闭弹窗
    private void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }
}
