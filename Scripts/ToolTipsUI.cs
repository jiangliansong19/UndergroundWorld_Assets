using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// toast提示语
/// </summary>
public class ToolTipsUI : MonoBehaviour
{
    public static ToolTipsUI Instance { private set; get; }

    [SerializeField] private RectTransform canvasRectTransform;

    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundRectTransform;
    private TooltipTimer tooltipTimer;
    private ToolTipPosition tooltipPosition;

    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        HandleFollowMouse();

        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    /// <summary>
    /// 处理tipsUI出现在屏幕之外的问题
    /// </summary>
    private void HandleFollowMouse()
    {
        if (tooltipPosition == null)
        {
            Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

            if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width)
            {
                anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
            }
            if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height)
            {
                anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
            }
            rectTransform.anchoredPosition = anchoredPosition;
        }
        else
        {
            rectTransform.anchoredPosition = tooltipPosition.position / canvasRectTransform.localScale.x;
        }
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8, 8);
        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    /// <summary>
    /// 显示tips
    /// </summary>
    /// <param name="tooltipText">tips文本内容</param>
    /// <param name="tooltipTimer">tips持续显示时间</param>
    /// <param name="position">tips显示的位置,屏幕坐标</param>
    public void Show(string tooltipText, TooltipTimer tooltipTimer = null, ToolTipPosition position = null)
    {
        this.tooltipTimer = tooltipTimer;
        this.tooltipPosition = position;

        gameObject.SetActive(true);
        SetText(tooltipText);
        HandleFollowMouse();
    }

    /// <summary>
    /// 隐藏tips
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //tips持续显示的时间
    public class TooltipTimer
    {
        public float timer;
    }

    public class ToolTipPosition
    {
        public Vector3 position;
    }
}
