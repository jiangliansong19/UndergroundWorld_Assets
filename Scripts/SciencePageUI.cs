using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SciencePageUI : MonoBehaviour
{
    [SerializeField] private Transform templateCellTransform;
    [SerializeField] private ScienceNodeSO emptyScienceNodeSO;


    private GridLayoutGroup gridLayoutGroup;

    private Transform scrollContentTransform;

    private ScienceTreeSO scienceTree;

    private List<SciencePageCellUI> scienceNodeTransforms;





    private void Awake()
    {
        scienceNodeTransforms = new List<SciencePageCellUI>();
        transform.gameObject.SetActive(false);

        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            HideSciencePage();
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
        });

        scienceTree = Resources.Load<ScienceTreeSO>(typeof(ScienceTreeSO).Name);

        scrollContentTransform = transform.Find("ScrollView").Find("Viewport").Find("Content");

        gridLayoutGroup = scrollContentTransform.GetComponent<GridLayoutGroup>();


        templateCellTransform.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int GetMaxHorizontalLengthOfScienceTree()
    {
        int max = 0;
        foreach (ScienceNodeListSO listSO in scienceTree.list)
        {
            max = Mathf.Max(max, listSO.list.Count);
        }
        return max;
    }


    public void createScienceNodes()
    {
        int maxCount = GetMaxHorizontalLengthOfScienceTree();
        gridLayoutGroup.constraintCount = maxCount;

        for (int i = 0; i < scienceTree.list.Count; i++)
        {
            for (int j = 0; j < maxCount; j++)
            {
                Transform newCellTransform = Instantiate(templateCellTransform, scrollContentTransform);
                newCellTransform.gameObject.SetActive(true);


                ScienceNodeSO nodeSO;
                if (j < scienceTree.list[i].list.Count)
                {
                    nodeSO = scienceTree.list[i].list[j];
                }
                else
                {
                    nodeSO = emptyScienceNodeSO;
                }

                SciencePageCellUI cellUI = newCellTransform.GetComponent<SciencePageCellUI>();
                scienceNodeTransforms.Add(cellUI);
                cellUI.SetScienceNodeSO(nodeSO);
                cellUI.SetNodeContentOnClick(DidSelectOnNode);
            }
        }
    }

    private int DidSelectOnNode(ScienceNodeSO nodeSO, SciencePageCellUI transform)
    {
        foreach (SciencePageCellUI cellUI in scienceNodeTransforms)
        {
            cellUI.SetSelected(false);
        }

        transform.SetSelected(true);
        return 0;
    }

    private void ShowPreviousScienceNode(ScienceNodeSO nodeSO)
    {

    }


    public void ShowSciencePage()
    {
        transform.gameObject.SetActive(true);
        createScienceNodes();
    }

    public void HideSciencePage()
    {
        transform.gameObject.SetActive(false);
    }
}
