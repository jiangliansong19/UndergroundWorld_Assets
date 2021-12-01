using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SciencePageUI : MonoBehaviour
{
    public static SciencePageUI Instance { private set; get; }
    [SerializeField] private RectTransform parentCanvas;
    [SerializeField] private Transform templateCellTransform;

    private SciencePageCellUI currentActiveCellUI;


    private GridLayoutGroup gridLayoutGroup;

    private Transform scrollContentTransform;

    private ScienceTreeSO scienceTree;

    private List<SciencePageCellUI> scienceNodeTransforms;





    private void Awake()
    {
        Instance = this;

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


    private void createScienceNodes()
    {
        gridLayoutGroup.constraintCount = scienceTree.list[0].list.Count;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < scienceTree.list[i].list.Count; j++)
            {
                Transform newCellTransform = Instantiate(templateCellTransform, scrollContentTransform);
                newCellTransform.gameObject.SetActive(true);

                ScienceNodeSO nodeSO = scienceTree.list[i].list[j];
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
