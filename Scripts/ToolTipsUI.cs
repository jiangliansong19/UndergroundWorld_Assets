using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// toast��ʾ��
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
    /// ����tipsUI��������Ļ֮�������
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
    /// ��ʾtips
    /// </summary>
    /// <param name="tooltipText">tips�ı�����</param>
    /// <param name="tooltipTimer">tips������ʾʱ��</param>
    /// <param name="position">tips��ʾ��λ��,��Ļ����</param>
    public void Show(string tooltipText, TooltipTimer tooltipTimer = null, ToolTipPosition position = null)
    {
        this.tooltipTimer = tooltipTimer;
        this.tooltipPosition = position;

        gameObject.SetActive(true);
        SetText(tooltipText);
        HandleFollowMouse();
    }

    /// <summary>
    /// ����tips
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    //tips������ʾ��ʱ��
    public class TooltipTimer
    {
        public float timer;
    }

    public class ToolTipPosition
    {
        public Vector3 position;
    }
}
