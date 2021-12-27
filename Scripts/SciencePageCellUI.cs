using System;
using UnityEngine;
using UnityEngine.UI;


//state: selected; 


public class SciencePageCellUI : MonoBehaviour
{
    private ScienceNodeSO _scienceNodeSO;

    private Transform nodeContentTransform;
    private Transform lineContentTransform;
    private Transform lineTransform;

    private Transform nodeBackgroundTransform;


    //lineContent
    private Transform topLeftLineTransform;
    private Transform topRightLineTransform;
    private Transform bottomLeftTransform;
    private Transform bottomRightTransform;
    private Transform centerLineTransform;


    private RectTransform templateUnlockIconRectTransform;

    private Transform _completionPercentBar;
    private RectTransform percentBarRectTransform;


    private Transform iconTemplate;
    private RectTransform iconTemplateRectTransform;
    private Image iconTemplateBackgroundImage;
    private Image iconTemplateIconImage;

    private float iconTemplateWidth = 20f;
    private float iconTmeplatePadding = 5f;


    private Outline outline;


    private Func<ScienceNodeSO, SciencePageCellUI, int> nodeContentOnClick;


    private void Awake()
    {
        nodeContentTransform = transform.Find("NodeContent");
        lineContentTransform = transform.Find("LineContent");
        lineTransform = transform.Find("OneLine");






        nodeBackgroundTransform = nodeContentTransform.Find("Background");


        iconTemplate = nodeContentTransform.Find("IconTemplate");
        iconTemplate.gameObject.SetActive(false);
        iconTemplateRectTransform = iconTemplate.GetComponent<RectTransform>();
        iconTemplateBackgroundImage = iconTemplate.Find("Background").GetComponent<Image>();
        iconTemplateIconImage = iconTemplate.Find("Icon").GetComponent<Image>();


        _completionPercentBar = nodeContentTransform.Find("PercentBar").Find("PercentInnerBar");
        percentBarRectTransform = _completionPercentBar.GetComponent<RectTransform>();
        



        centerLineTransform = lineContentTransform.Find("Center");
        topLeftLineTransform = lineContentTransform.Find("TopLeft");
        topRightLineTransform = lineContentTransform.Find("TopRight");
        bottomLeftTransform = lineContentTransform.Find("BottomLeft");
        bottomRightTransform = lineContentTransform.Find("BottomRight");
        





        outline = nodeBackgroundTransform.GetComponent<Outline>();




    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateActiveCompletePercent(0.6f);

        Button nodeContentButton = nodeContentTransform.GetComponent<Button>();
        nodeContentButton.onClick.AddListener(OnClickOnNodeContent);

        MouseEnterAndExits enterAndExits = nodeContentTransform.GetComponent<MouseEnterAndExits>();
        enterAndExits.OnMouseEnterEvent += ScienceCellUIEnterAndExits_OnMouseEnterEvent;
        enterAndExits.OnMouseExitEvent += ScienceCellUIEnterAndExits_OnMouseExitEvent;

    }

    private void ScienceCellUIEnterAndExits_OnMouseExitEvent(object sender, EventArgs e)
    {
        ToolTipsUI.Instance.Hide();
    }

    private void ScienceCellUIEnterAndExits_OnMouseEnterEvent(object sender, EventArgs e)
    {
        ToolTipsUI.Instance.Show(GameProjectSettings.IsDebug ? _scienceNodeSO.remark : _scienceNodeSO.nodeDesc);
    }


    // Update is called once per frame
    void Update()
    {
        //only to refresh is completed percent.









        if (_scienceNodeSO == ScienceManager.Instance.GetActiveScienceNodeSO())
        {
            float percent = ScienceManager.Instance.GetCompletedPercent(_scienceNodeSO);
            UpdateActiveCompletePercent(percent);

            _completionPercentBar.gameObject.SetActive(true);
            outline.effectColor = Color.red;
        }
        else
        {

            outline.effectColor = Color.white;
            _completionPercentBar.gameObject.SetActive(false);
        }
    }


    private void OnClickOnNodeContent()
    {
        nodeContentOnClick(_scienceNodeSO, this);


        DialogUI.Create().ShowDialog("Alert", _scienceNodeSO.nodeDesc, () => {

            ScienceManager.Instance.SetActiveScienceNodeSO(_scienceNodeSO);
            Debug.Log("dialog click on ok");

            return 0;
        }, () => {

            Debug.Log("dialog click on cancel");
            return 0;
        });
    }






    public void SetScienceNodeSO(ScienceNodeSO nodeSO)
    {
        this._scienceNodeSO = nodeSO;

        if (nodeSO.nodeType == ScienceCategoryType.Empty)
        {
            lineTransform.gameObject.SetActive(false);
            lineContentTransform.gameObject.SetActive(false);
            nodeContentTransform.gameObject.SetActive(false);
        }
        else
        {
            CreateLeftNodeContent(nodeSO);
            CreateRightLineContent(nodeSO);
        }
    }

    public ScienceNodeSO GetScienceNodeSO()
    {
        return _scienceNodeSO;
    }






    private void CreateLeftNodeContent(ScienceNodeSO nodeSO)
    {

        if (nodeSO.leftPartType == ScienceLeftPartType.Line)
        {
            lineTransform.gameObject.SetActive(true);
            nodeContentTransform.gameObject.SetActive(false);
        }
        else if (nodeSO.leftPartType == ScienceLeftPartType.Empty)
        {
            lineTransform.gameObject.SetActive(false);
            nodeContentTransform.gameObject.SetActive(false);
        }
        else
        {
            lineTransform.gameObject.SetActive(false);
            nodeContentTransform.gameObject.SetActive(true);

            if (nodeSO.nodeDesc != null)
            {
                nodeContentTransform.Find("Text").GetComponent<Text>().text = nodeSO.nodeDesc;
            }



            if (_scienceNodeSO.unlockBuildingTypeSO != null)
            {
                int i = 0;
                foreach (BuildingTypeSO typeSO in _scienceNodeSO.unlockBuildingTypeSO)
                {
                    Transform newTransform = Instantiate(iconTemplate, nodeContentTransform);
                    newTransform.gameObject.SetActive(true);
                    newTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2((iconTemplateWidth + iconTmeplatePadding) * i, 0);
                    newTransform.Find("Icon").GetComponent<Image>().sprite = typeSO.sprite;
                    MouseEnterAndExits enterAndExits = newTransform.GetComponent<MouseEnterAndExits>();
                    enterAndExits.OnMouseEnterEvent += (object sender, EventArgs e) =>
                    {
                        ToolTipsUI.Instance.Show(typeSO.buildingName);
                    };
                    enterAndExits.OnMouseExitEvent += (object sender, EventArgs e) =>
                    {
                        ToolTipsUI.Instance.Show(GameProjectSettings.IsDebug ? nodeSO.remark : nodeSO.nodeDesc);
                    };
                    i++;
                }
            }

            UpdateActiveCompletePercent(ScienceManager.Instance.GetCompletedPercent(_scienceNodeSO));
        }
    }

    private void EnterAndExits_OnMouseEnterEvent(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void CreateRightLineContent(ScienceNodeSO nodeSO)
    {
        lineContentTransform.gameObject.SetActive(true);
        centerLineTransform.gameObject.SetActive(false);
        topLeftLineTransform.gameObject.SetActive(false);
        topRightLineTransform.gameObject.SetActive(false);
        bottomLeftTransform.gameObject.SetActive(false);
        bottomRightTransform.gameObject.SetActive(false);

        foreach (ScienceRightPartType part in nodeSO.rightPartTypes)
        {
            switch (part)
            {
                case ScienceRightPartType.Line:
                    centerLineTransform.gameObject.SetActive(true);
                    break;
                case ScienceRightPartType.TopLeft:
                    topLeftLineTransform.gameObject.SetActive(true);
                    break;
                case ScienceRightPartType.TopRight:
                    topRightLineTransform.gameObject.SetActive(true);
                    break;
                case ScienceRightPartType.BottomLeft:
                    bottomLeftTransform.gameObject.SetActive(true);
                    break;
                case ScienceRightPartType.BottomRight:
                    bottomRightTransform.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

    }




    public void UpdateActiveCompletePercent(float percent)
    {
        percentBarRectTransform.localScale = new Vector2(percent, 1);

        if (percent == 1.0)
        {
            nodeBackgroundTransform.GetComponent<Image>().color = Color.blue;
        }

    }


    
    public void SetSelected(bool isSelect)
    {
        if (_scienceNodeSO.nodeType == ScienceCategoryType.Empty)
        {
            return;
        }

        if (isSelect)
        {
            outline.effectColor = Color.cyan;
        }
        else
        {
            outline.effectColor = Color.white;
        }
    }






    public void SetNodeContentOnClick(Func<ScienceNodeSO, SciencePageCellUI, int> func)
    {
        nodeContentOnClick = func;
    }
}
