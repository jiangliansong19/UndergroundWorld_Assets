using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;




public enum BottomMenuType
{
    Demolish,
    House,
    Eat,
    Drink,
    Transport,
    Industry,
    Energy,
    Other,
    Decorate,
    Priority,
    GreatProject,
    Science,
    RuleAndDecision,
    ExplorTeam,
    Setting,
}


public class BottomMenuUI : MonoBehaviour
{
    private BottomMenuSO bottomMenu;

    [SerializeField] List<BuildingTypeSO> ignoreBuildingList;

    private float padding = 2f;
    private float width = 23f;

    private Transform menuTemplate;
    private Transform buttonTemplate;

    private Transform lastSubMenu;

    private List<Transform> tmpTransforms;
    private List<Transform> architectureTransforms;


    private void Awake()
    {

        bottomMenu = Resources.Load<BottomMenuSO>(typeof(BottomMenuSO).Name);

        tmpTransforms = new List<Transform>();
        architectureTransforms = new List<Transform>();

        menuTemplate = transform.Find("BuildMenu");
        buttonTemplate = menuTemplate.Find("BuildMenuItem");

        buttonTemplate.gameObject.SetActive(false);
        menuTemplate.gameObject.SetActive(false);

        ShowBuildMenuItems();
    }


    //一级目录
    private void ShowBuildMenuItems()
    {
        Transform menuTransform = Instantiate(menuTemplate, transform);
        menuTransform.gameObject.SetActive(true);
        
        float menuWidth = bottomMenu.menuList.Count * (width + padding) + padding;
        RectTransform menuRectTransform = menuTransform.GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2(menuWidth, (width + padding * 2));
        menuRectTransform.anchoredPosition = new Vector2(-menuWidth, 0);

        for (int i = 0; i < bottomMenu.menuList.Count; i++)
        {
            BottomMenuSO.BottomMenuItem item = bottomMenu.menuList[i];

            Transform buttonTransform = Instantiate(buttonTemplate, menuTransform);
            buttonTransform.gameObject.SetActive(true);
            architectureTransforms.Add(buttonTransform);

            buttonTransform.Find("Icon").GetComponent<Image>().sprite = item.sprite;
            buttonTransform.Find("Selected").gameObject.SetActive(false);

            Vector3 anchoredPosition = new Vector3(i * (width + padding) + padding, padding, 0);
            RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = anchoredPosition;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (lastSubMenu != null)
                {
                    Destroy(lastSubMenu.gameObject);
                }
                


                foreach (Transform itemTransform in architectureTransforms)
                {
                    itemTransform.Find("Selected").gameObject.SetActive(false);
                }
                buttonTransform.Find("Selected").gameObject.SetActive(true);

                if (tmpTransforms.Count > 0)
                {
                    foreach (Transform item in tmpTransforms)
                    {
                        Destroy(item.gameObject);
                    }
                    tmpTransforms = new List<Transform>();
                }

                //设置
                if (item.type == BottomMenuType.Setting)
                {
                    SettingPageUI.Instance.ShowSettingPageUI();
                }
                //科技
                else if (item.type == BottomMenuType.Science)
                {
                    ScienceManager.Instance.ShowSciencePage();
                }
                //探索队
                else if (item.type == BottomMenuType.ExplorTeam)
                {
                    ExploreTeamManager.Instance.ShowExploreTeamPanel();
                }
                //伟大工程
                else if (item.type == BottomMenuType.GreatProject)
                {
                    ExploreTeamManager.Instance.ShowExploreTeamPanel();
                }
                else
                {
                    OnClickBuildMenuItem(item, buttonTransform);
                }


                
                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
            });

            //show info tips
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseEnterEvent += (object sender, System.EventArgs e) =>
            {
                Vector3 tipsPosition = new Vector3(buttonTransform.position.x, buttonTransform.position.y + 0, 0);
                ToolTipsUI.Instance.Show(item.name, null, new ToolTipsUI.ToolTipPosition { position = buttonTransform.position });
            };
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseExitEvent += (object sender, System.EventArgs e) =>
            {
                ToolTipsUI.Instance.Hide();
            };
        }
    }


    //二级目录
    private void OnClickBuildMenuItem(BottomMenuSO.BottomMenuItem item, Transform aTransform)
    {

        Transform subMenuTransform = Instantiate(menuTemplate, transform);
        subMenuTransform.gameObject.SetActive(true);
        float menuWidth = item.buildingList.Count * (width + padding) + padding;
        RectTransform subRectTransform = subMenuTransform.GetComponent<RectTransform>();
        subRectTransform.sizeDelta = new Vector2(menuWidth, (width + padding * 2));
        Vector2 offsetAnch = new Vector2(-(width + padding) * (bottomMenu.menuList.Count - bottomMenu.menuList.IndexOf(item)) - padding, width + padding);
        subRectTransform.anchoredPosition = offsetAnch;

        lastSubMenu = subMenuTransform;


        for (int i = 0; i < item.buildingList.Count; i++)
        {
            BuildingTypeSO buildingTypeSO = item.buildingList[i];

            if (ignoreBuildingList.Contains(buildingTypeSO))
            {
                continue;
            }

            //todo: if science is no unlock, hide related icon;
            //if (!ScienceManager.Instance.isBuildingUnlock(buildingTypeSO))
            //{
            //    continue;
            //}


            Transform buttonTransform = Instantiate(buttonTemplate, subMenuTransform);
            buttonTransform.gameObject.SetActive(true);
            tmpTransforms.Add(buttonTransform);

            buttonTransform.Find("Icon").GetComponent<Image>().sprite = buildingTypeSO.sprite;
            buttonTransform.Find("Selected").gameObject.SetActive(false);

            Vector3 anchoredPosition = new Vector2(i * (width + padding) + padding, padding);
            RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = anchoredPosition;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {

                if (item.type == BottomMenuType.Demolish)
                {
                    DemolishManager.Instance.SetDemolishType(buildingTypeSO);
                    BuildingManager.Instance.SetActiveBuildingTypeSO(null);
                }
                else
                {
                    DemolishManager.Instance.SetDemolishType(null);
                    BuildingManager.Instance.SetActiveBuildingTypeSO(buildingTypeSO);
                }

                
                //change selected state
                foreach (Transform item in tmpTransforms)
                {
                    item.Find("Selected").gameObject.SetActive(false);
                }
                buttonTransform.Find("Selected").gameObject.SetActive(true);


                SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
            });

            //show info tips
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseEnterEvent += (object sender, System.EventArgs e) =>
            {
                Vector3 tipsPosition = new Vector3(buttonTransform.position.x, buttonTransform.position.y + 0, 0);
                ToolTipsUI.Instance.Show(buildingTypeSO.buildingName, null, new ToolTipsUI.ToolTipPosition { position = tipsPosition });
            };
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseExitEvent += (object sender, System.EventArgs e) =>
            {
                ToolTipsUI.Instance.Hide();
            };
        }
    }
}
