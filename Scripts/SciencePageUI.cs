using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SciencePageUI : MonoBehaviour
{
    public static SciencePageUI Instance { private set; get; }
    [SerializeField] private RectTransform parentCanvas;
    [SerializeField] private Transform templateCellTransform;



    private GridLayoutGroup gridLayoutGroup;

    private Transform scrollContentTransform;

    private ScienceTreeSO scienceTree;

    private List<SciencePageCellUI> scienceNodeTransformDict;

    private void Awake()
    {
        Instance = this;

        scienceNodeTransformDict = new List<SciencePageCellUI>();
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
        createScienceNodes();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private int GetMaxColumnCount()
    {
        int max = 0;
        foreach (ScienceNodeListSO nodeListSO in scienceTree.list)
        {
            max = Mathf.Max(nodeListSO.list.Count, max);
        }
        return max;
    }

    private void createScienceNodes()
    {
        int columnCount = GetMaxColumnCount();
        for (int i = 0; i < scienceTree.list.Count; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                Transform newCellTransform = Instantiate(templateCellTransform, scrollContentTransform);
                if (scienceTree.list[i].list != null && scienceTree.list[i].list.Count > j)
                {
                    newCellTransform.gameObject.SetActive(true);
                    newCellTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(180 * i, 100 * j);

                    

                    SciencePageCellUI cellUI = newCellTransform.GetComponent<SciencePageCellUI>();

                    Object value = scienceTree.list[i].list[j];

                    if (value != null)
                    {
                        scienceNodeTransformDict.Add(cellUI);

                        cellUI.OnSelectCellUIEvent += CellUI_OnSelectCellUIEvent;


                        cellUI.SetScienceNodeSO(scienceTree.list[i].list[j]);
                    }


                }
            }
        }
    }

    private void CellUI_OnSelectCellUIEvent(object sender, SciencePageCellUI.SciencePageCellDidSelectedArgs e)
    {
        
    }

    public void ShowSciencePage()
    {
        transform.gameObject.SetActive(true);
    }

    public void HideSciencePage()
    {
        transform.gameObject.SetActive(false);
    }
}
