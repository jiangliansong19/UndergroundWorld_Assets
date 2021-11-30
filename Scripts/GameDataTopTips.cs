using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDataTopTips : MonoBehaviour
{

    private Text totalMoneyText;
    private Text incomePerDayText;

    private void Awake()
    {
        totalMoneyText = transform.Find("TotalMoney").Find("Content").GetComponent<Text>();
        incomePerDayText = transform.Find("IncomePerDay").Find("Content").GetComponent<Text>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDataManager.Instance != null)
        {
            totalMoneyText.text = GameDataManager.Instance.totalMoney.ToString();
            incomePerDayText.text = GameDataManager.Instance.IncomePerDay.ToString();
        }
    }
}
