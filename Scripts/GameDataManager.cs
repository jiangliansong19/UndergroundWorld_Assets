using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { private set; get; }


    [HideInInspector] public int gameSpeed = 1;
    [HideInInspector] public double totalMoney;
    [HideInInspector] public double IncomePerDay;

    public event EventHandler GameDataHasChanged;


    private float timer, maxTime;


    private void Awake()
    {
        Instance = this;
        timer = 0;
        maxTime = 2;
        totalMoney = 1000000;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += maxTime;

            UpdateGameMoney();
        }
    }


    private void UpdateGameMoney()
    {
        totalMoney += IncomePerDay;
    }
}
