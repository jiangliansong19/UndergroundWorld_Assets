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
    EatAndDrink,
    Transport,
    Industry,
    Energy,
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
    private float width = 35f;
    private Transform buttonTemplate;

    private List<Transform> tmpTransforms;
    private List<Transform> architectureTransforms;






    private void Awake()
    {

        bottomMenu = Resources.Load<BottomMenuSO>(typeof(BottomMenuSO).Name);

        tmpTransforms = new List<Transform>();
        architectureTransforms = new List<Transform>();

        buttonTemplate = transform.Find("BuildMenuItem");
        buttonTemplate.gameObject.SetActive(false);

        ShowBuildMenuItems();
    }

    /// <summary>
    /// 展示菜单
    /// </summary>
    private void ShowBuildMenuItems()
    {
        int i = 0;
        transform.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(-bottomMenu.menuList.Count * (width + padding), padding);

        foreach (BottomMenuSO.BottomMenuItem item in bottomMenu.menuList)
        {
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);
            architectureTransforms.Add(buttonTransform);

            buttonTransform.Find("Icon").GetComponent<Image>().sprite = item.sprite;
            buttonTransform.Find("Selected").gameObject.SetActive(false);

            Vector3 anchoredPosition = new Vector3(i * (width + padding), 0, 0);
            RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = anchoredPosition;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
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
                    OnClickBuildMenuItem(item);
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
    private void OnClickBuildMenuItem(BottomMenuSO.BottomMenuItem item)
    {
        int i = 0;
        foreach (BuildingTypeSO buildingTypeSO in item.buildingList)
        {
            if (ignoreBuildingList.Contains(buildingTypeSO))
            {
                continue;
            }
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);
            tmpTransforms.Add(buttonTransform);

            buttonTransform.Find("Icon").GetComponent<Image>().sprite = buildingTypeSO.sprite;
            buttonTransform.Find("Selected").gameObject.SetActive(false);

            Vector3 anchoredPosition = new Vector2(i * (width + padding), width + padding);
            RectTransform buttonRectTransform = buttonTransform.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = anchoredPosition;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (item.type == BottomMenuType.Demolish)
                {
                    DemolishManager.Instance.SetDemolishType(buildingTypeSO);
                }

                else
                {
                    BuildingManager.Instance.SetActiveBuildingTypeSO(buildingTypeSO);
                }

                

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
