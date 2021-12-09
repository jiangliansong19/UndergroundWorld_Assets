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


    private float padding = 5f;
    private float width = 30f;

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

    /// <summary>
    /// 展示菜单
    /// </summary>
    private void ShowBuildMenuItems()
    {
        Transform menuTransform = Instantiate(menuTemplate, transform);
        menuTransform.gameObject.SetActive(true);
        
        float menuWidth = bottomMenu.menuList.Count * (width + padding) + padding;
        RectTransform menuRectTransform = menuTransform.GetComponent<RectTransform>();
        menuRectTransform.sizeDelta = new Vector2(menuWidth, (width + padding * 2));
        menuRectTransform.anchoredPosition = new Vector2(-menuWidth, 0);

        int i = 0;
        foreach (BottomMenuSO.BottomMenuItem item in bottomMenu.menuList)
        {
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


                if (item.type == BottomMenuType.Setting)
                {
                    Debug.Log("enter setting page");
                }
                else if (item.type == BottomMenuType.Science)
                {
                    ScienceManager.Instance.ShowSciencePage();
                }
                else if (item.type == BottomMenuType.ExplorTeam)
                {
                    ExploreTeamManager.Instance.ShowExploreTeamPanel();
                }
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

            
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseEnterEvent += (object sender, System.EventArgs e) =>
            {
                Vector3 tipsPosition = new Vector3(buttonTransform.position.x, buttonTransform.position.y + 0, 0);
                ToolTipsUI.Instance.Show(item.name, null, new ToolTipsUI.ToolTipPosition { position = buttonTransform.position });
            };
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseExitEvent += (object sender, System.EventArgs e) =>
            {
                ToolTipsUI.Instance.Hide();
            };

            i++;
        }
    }

    /// <summary>
    /// 点击菜单
    /// </summary>
    /// <param name="item"></param>
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


        int i = 0;
        foreach (BuildingTypeSO buildingTypeSO in item.buildingList)
        {
            if (ignoreBuildingList.Contains(buildingTypeSO))
            {
                continue;
            }
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

            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseEnterEvent += (object sender, System.EventArgs e) =>
            {
                Vector3 tipsPosition = new Vector3(buttonTransform.position.x, buttonTransform.position.y + 0, 0);
                ToolTipsUI.Instance.Show(buildingTypeSO.GetBuildingDescription(), null, new ToolTipsUI.ToolTipPosition { position = tipsPosition });
            };
            buttonTransform.GetComponent<MouseEnterAndExits>().OnMouseExitEvent += (object sender, System.EventArgs e) =>
            {
                ToolTipsUI.Instance.Hide();
            };

            i++;
        }
    }
}
