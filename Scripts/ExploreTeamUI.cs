using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreTeamUI : MonoBehaviour
{
    [SerializeField] private Transform addMoreExploreTeamTransform;
    [SerializeField] private Transform templateExploreTeamTransform;
    [SerializeField] private Transform contentTransform;

    private List<ExploreTeam> exploreTeams;

    private Button closeButton;




    private float cellHeight = 100f;
    private float cellPadding = 10f;

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.SetActive(false);


        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(() => {
            Hide();
        });

        addMoreExploreTeamTransform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            ExploreTeam data = new ExploreTeam();

            ExploreTeamManager.Instance.AddExploreTeam(data);

            CreateOneExploreTeam(data);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }







    private void CreateExploreTeamsPanel()
    {
        templateExploreTeamTransform.gameObject.SetActive(false);

        for (int i = 0; i < exploreTeams.Count; i++)
        {
            Transform newTransform = Instantiate(templateExploreTeamTransform, contentTransform);
            newTransform.gameObject.SetActive(true);
        }
    }

    private void CreateOneExploreTeam(ExploreTeam data)
    {

        Transform newTransform = Instantiate(templateExploreTeamTransform, contentTransform);
        newTransform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    public void Show(List<ExploreTeam> teams)
    {
        transform.gameObject.SetActive(true);

        this.exploreTeams = teams;

        CreateExploreTeamsPanel();
    }
}
