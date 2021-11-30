using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SciencePageCellUI : MonoBehaviour
{
    public event EventHandler<SciencePageCellDidSelectedArgs> OnSelectCellUIEvent;
    public class SciencePageCellDidSelectedArgs
    {
        public SciencePageCellUI cellUI;
        public ScienceNodeSO scienceNodeSO;
    }


    private ScienceNodeSO scienceNodeSO;

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

    private RectTransform percentBarRectTransform;


    private Transform iconTemplate;
    private RectTransform iconTemplateRectTransform;
    private Image iconTemplateBackgroundImage;
    private Image iconTemplateIconImage;

    private float iconTemplateWidth = 20f;
    private float iconTmeplatePadding = 5f;


    private Outline outline;


    private Func<int> nodeContentOnClick;


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

        percentBarRectTransform = nodeContentTransform.Find("PercentBar").Find("PercentInnerBar").GetComponent<RectTransform>();




        centerLineTransform = lineContentTransform.Find("Center");
        topLeftLineTransform = lineContentTransform.Find("TopLeft");
        topRightLineTransform = lineContentTransform.Find("TopRight");
        bottomLeftTransform = lineContentTransform.Find("BottomLeft");
        bottomRightTransform = lineContentTransform.Find("BottomRight");
        





        outline = nodeBackgroundTransform.GetComponent<Outline>();



        nodeContentTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            DialogUI.Instance.ShowDialog("Alert", scienceNodeSO.nodeDesc, () => {

                OnSelectCellUIEvent?.Invoke(this, new SciencePageCellDidSelectedArgs()
                {
                    cellUI = this,
                    scienceNodeSO = this.scienceNodeSO
                });

                SetSelected(true);
                Debug.Log("dialog click on ok");

                return 0;
            }, () => {

                Debug.Log("dialog click on cancel");
                return 0;
            });

        });
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateCompletePercent(0.6f);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScienceNodeSO(ScienceNodeSO nodeSO)
    {
        this.scienceNodeSO = nodeSO;

        if (nodeSO.nodeType == ScienceCategoryType.Line)
        {
            lineTransform.gameObject.SetActive(true);
            lineContentTransform.gameObject.SetActive(false);
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
            


            if (scienceNodeSO.unlockBuildingTypeSO != null)
            {
                int i = 0;
                foreach (BuildingTypeSO typeSO in scienceNodeSO.unlockBuildingTypeSO)
                {
                    Transform newTransform = Instantiate(iconTemplate, nodeContentTransform);
                    newTransform.gameObject.SetActive(true);
                    newTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(iconTemplateWidth * i, 0);
                    newTransform.Find("Icon").GetComponent<Image>().sprite = typeSO.sprite;
                    i++;
                }
            }





            lineContentTransform.gameObject.SetActive(true);
            centerLineTransform.gameObject.SetActive(false);
            topLeftLineTransform.gameObject.SetActive(false);
            topRightLineTransform.gameObject.SetActive(false);
            bottomLeftTransform.gameObject.SetActive(false);
            bottomRightTransform.gameObject.SetActive(false);

            switch (nodeSO.rightPartType)
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

    public void UpdateCompletePercent(float percent)
    {
        percentBarRectTransform.localScale = new Vector2(percent, 1);
    }

    public void SetSelected(bool isSelect)
    {
        if (isSelect)
        {
            outline.effectColor = Color.yellow;
        }
        else
        {
            outline.effectColor = Color.gray;
        }
    }

    public void SetNodeContentOnClick(Func<int> func)
    {
        nodeContentOnClick = func;
    }
}
