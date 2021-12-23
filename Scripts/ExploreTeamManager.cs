using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreTeamManager : MonoBehaviour
{
    public static ExploreTeamManager Instance { private set; get; }

    public bool isShowing { get; private set; }

    [SerializeField] private ExploreTeamUI exploreTeamUI;

    private List<ExploreTeam> exploreTeams;

    private void Awake()
    {
        Instance = this;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        exploreTeams = new List<ExploreTeam>();
        exploreTeams.Add(new ExploreTeam());
        exploreTeams.Add(new ExploreTeam());
        exploreTeams.Add(new ExploreTeam());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddExploreTeams(List<ExploreTeam> datas)
    {
        foreach (ExploreTeam item in datas)
        {
            exploreTeams.Add(item);
        }
    }

    public List<ExploreTeam> GetExploreTeams()
    {
        return this.exploreTeams;
    }

    public void AddExploreTeam(ExploreTeam data)
    {
        exploreTeams.Add(data);
    }

    public void ShowExploreTeamPanel()
    {
        isShowing = true;

        CameraHandler.Instance.SetHandleMouseEnable(false);

        exploreTeamUI.Show(exploreTeams);
    }

    public void HideExploreTeamPanel()
    {
        isShowing = false;

        CameraHandler.Instance.SetHandleMouseEnable(true);

        exploreTeamUI.Hide();
    }
}
