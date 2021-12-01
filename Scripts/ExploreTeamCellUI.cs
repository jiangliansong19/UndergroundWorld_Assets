using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreTeamCellUI : MonoBehaviour
{
    private ExploreTeam exploreTeamData;

    private Button goBackButton;
    private Button goStartButton;

    // Start is called before the first frame update
    void Start()
    {
        goStartButton = transform.Find("GoStart").GetComponent<Button>();
        goBackButton = transform.Find("GoBack").GetComponent<Button>();

        goStartButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
            DialogUI.Create().ShowDialog("Alert", "���", 
                () => { 

                    return 0; 
                }, 
                () => { 
                    return 0; 
                });
        });

        goBackButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
            DialogUI.Create().ShowDialog("Alert", "����",
                () => {

                    return 0;
                },
                () => {
                    return 0;
                });
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetExploreTeam(ExploreTeam data)
    {
        exploreTeamData = data;


    }
}
