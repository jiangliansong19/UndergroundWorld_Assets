using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// show,hidden,canActivate,
/// </summary>
public class ScienceManager : MonoBehaviour
{

    public static ScienceManager Instance { private set;get; }

    [SerializeField] private SciencePageUI sciencePageUI;

    private ScienceNodeSO activeScienceNodeSO;

    public List<ScienceNodeSO> completedScienceNodeSOs;


    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public bool CanExecuteScienceNode(ScienceNodeSO nodeSO)
    {
        if (nodeSO.previousNodes == null || nodeSO.previousNodes.Count == 0)
        {
            return true;
        }

        foreach (ScienceNodeSO node in nodeSO.previousNodes)
        {
            if (!completedScienceNodeSOs.Contains(node))
            {
                return false;
            }
        }

        return false;
    }


    public void ShowSciencePage()
    {
        sciencePageUI.ShowSciencePage();
    }

    public void HideSciencePage()
    {
        sciencePageUI.HideSciencePage();
    }

    public void SetActiveScienceNodeSO(ScienceNodeSO nodeSO)
    {
        activeScienceNodeSO = nodeSO;
    }

    public ScienceNodeSO GetActiveScienceNodeSO()
    {
        return activeScienceNodeSO;
    }
}
