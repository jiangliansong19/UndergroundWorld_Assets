using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimeSpeedUI : MonoBehaviour
{

    public static TimeSpeedUI Instance { private set; get; }
    public event EventHandler OnGameTimeSpeedChangedEvent;

    private Transform pauseTrasform;
    private Transform oneSpeedTranform;
    private Transform twoSpeedTransform;
    private Transform threeSpeedTransform;
    private int timeSpeedScale;

    private void Awake()
    {
        Instance = this;
        pauseTrasform = transform.Find("Pause");
        oneSpeedTranform = transform.Find("OneSpeed");
        twoSpeedTransform = transform.Find("TwoSpeed");
        threeSpeedTransform = transform.Find("ThreeSpeed");

        pauseTrasform.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeTimeSpeed(0);
            ChangeIconColor(pauseTrasform, Color.red);
        });
        oneSpeedTranform.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeTimeSpeed(1);
            ChangeIconColor(oneSpeedTranform, new Color(0, 0.7f, 0, 1));
        });
        twoSpeedTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeTimeSpeed(2);
            ChangeIconColor(twoSpeedTransform, new Color(0, 0.85f, 0, 1));
        });
        threeSpeedTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeTimeSpeed(3);
            ChangeIconColor(threeSpeedTransform, new Color(0, 1f, 0, 1));
        });
    }

    private void ChangeTimeSpeed(int speedScale)
    {

        this.timeSpeedScale = speedScale;
        OnGameTimeSpeedChangedEvent?.Invoke(this, EventArgs.Empty);

        SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
    }

    private void ChangeIconColor(Transform trans, Color color)
    {
        Transform[] all = new Transform[] { oneSpeedTranform, twoSpeedTransform, threeSpeedTransform, pauseTrasform };
        foreach (Transform item in all)
        {
            Image image0 = item.Find("Icon").GetComponent<Image>();
            if (item == trans)
            {
                if (image0 != null)
                {
                    image0.color = color;
                }
            }
            else
            {
                if (image0 != null)
                {
                    image0.color = Color.white;
                }
            }

        }
    }

    public int GetTimeSpeed()
    {
        return this.timeSpeedScale;
    }
}
