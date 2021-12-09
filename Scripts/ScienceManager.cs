using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// show,hidden,canActivate,
/// </summary>
public class ScienceManager : MonoBehaviour
{

    public static ScienceManager Instance { private set;get; }

    [SerializeField] private SciencePageUI _sciencePageUI;

    private ScienceNodeSO activeScienceNodeSO;


    //to record all science node completed percent.
    private Dictionary<ScienceNodeSO, float> scienceNodeCompletePercentsDict;




    private void Awake()
    {
        Instance = this;


        //read local data scienceNodeCompletePercent
        //todo

        ScienceTreeSO scienceTree = Resources.Load<ScienceTreeSO>(typeof(ScienceTreeSO).Name);
        scienceNodeCompletePercentsDict = new Dictionary<ScienceNodeSO, float>();
        foreach (ScienceNodeListSO listSO in scienceTree.list)
        {
            foreach (ScienceNodeSO nodeSO in listSO.list)
            {
                scienceNodeCompletePercentsDict[nodeSO] = 0f;
            }
        }



    }


    // Start is called before the first frame update
    void Start()
    {
        ResourcesManager.Instance.OnResourcesChangedEvent += Instance_OnResourcesChangedEvent;
    }

    private void Instance_OnResourcesChangedEvent(object sender, ResourcesManager.ResourceChangeAmountArgs e)
    {
        if (e.typeAmount.resourceType.type == ResourceType.ScienceScore)
        {
            UpdateCompletedPercent();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        
    }

    private void UpdateCompletedPercent()
    {
        if (activeScienceNodeSO != null)
        {
            long score = ResourcesManager.Instance.GetResourceAmount(ResourceType.ScienceScore);

            if (score == 0)
            {
                return;
            }

            float percent = scienceNodeCompletePercentsDict[activeScienceNodeSO];
            long needScore = (long)(activeScienceNodeSO.needScienceScore * (1.0f - percent));

            if (score >= needScore)
            {

                ResourcesManager.Instance.AddResource(ResourceType.ScienceScore, -(score - needScore));

                scienceNodeCompletePercentsDict[activeScienceNodeSO] = 1f;

                activeScienceNodeSO = null;
            }
            else
            {
                ResourcesManager.Instance.AddResource(ResourceType.ScienceScore, -score);

                scienceNodeCompletePercentsDict[activeScienceNodeSO] += (float)(score / activeScienceNodeSO.needScienceScore);
            }

            _sciencePageUI.UpdateCompletePercent(activeScienceNodeSO, scienceNodeCompletePercentsDict[activeScienceNodeSO]);
        }
    }


    public bool CanExecuteScienceNode(ScienceNodeSO nodeSO)
    {
        if (nodeSO.previousNodes == null || nodeSO.previousNodes.Count == 0)
        {
            return true;
        }

        foreach (ScienceNodeSO node in nodeSO.previousNodes)
        {
            if (scienceNodeCompletePercentsDict[node] < 1.00)
            {
                return false;
            }
        }

        return false;
    }




    public void ShowSciencePage()
    {
        _sciencePageUI.ShowSciencePage();
    }

    public void HideSciencePage()
    {
        _sciencePageUI.HideSciencePage();
    }




    public void SetActiveScienceNodeSO(ScienceNodeSO nodeSO)
    {
        activeScienceNodeSO = nodeSO;
    }

    public ScienceNodeSO GetActiveScienceNodeSO()
    {
        return activeScienceNodeSO;
    }




    public float GetCompletedPercent(ScienceNodeSO node)
    {
        return scienceNodeCompletePercentsDict[node];
    }


}
