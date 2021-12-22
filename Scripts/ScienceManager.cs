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


    private ScienceTreeSO _scienceTree;


    //to record all science node completed percent.
    private Dictionary<ScienceNodeSO, float> scienceNodeCompletePercentsDict;


    //saved in local
    private Dictionary<string, float> localScienceNodePercentsDict;


    private void Awake()
    {
        Instance = this;


        //read local data scienceNodeCompletePercent
        //todo

        _scienceTree = Resources.Load<ScienceTreeSO>(typeof(ScienceTreeSO).Name);
        scienceNodeCompletePercentsDict = new Dictionary<ScienceNodeSO, float>();

        foreach (ScienceNodeListSO listSO in _scienceTree.list)
        {
            foreach (ScienceNodeSO nodeSO in listSO.list)
            {
                scienceNodeCompletePercentsDict[nodeSO] = localScienceNodePercentsDict[nodeSO.nodeType.ToString()];
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

                _sciencePageUI.UpdateCompletePercent(activeScienceNodeSO, 1.0f);

                activeScienceNodeSO = null;
            }
            else
            {
                ResourcesManager.Instance.AddResource(ResourceType.ScienceScore, -score);

                scienceNodeCompletePercentsDict[activeScienceNodeSO] += (float)score / (float)activeScienceNodeSO.needScienceScore;

                _sciencePageUI.UpdateCompletePercent(activeScienceNodeSO, scienceNodeCompletePercentsDict[activeScienceNodeSO]);
            }

        }
    }


    //check whether the previous science node is completed
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



    //search a science node completion percent
    public float GetCompletedPercent(ScienceNodeSO node)
    {
        return scienceNodeCompletePercentsDict[node];
    }

    //search whether a building is unclock.
    public bool isBuildingUnlock(BuildingTypeSO typeSO)
    {
        foreach (ScienceNodeListSO listSO in _scienceTree.list)
        {
            foreach (ScienceNodeSO nodeSO in listSO.list)
            {
                if (nodeSO.unlockBuildingTypeSO.Contains(typeSO))
                {
                    if (GetCompletedPercent(nodeSO) == 1)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        return false;
    }
}
