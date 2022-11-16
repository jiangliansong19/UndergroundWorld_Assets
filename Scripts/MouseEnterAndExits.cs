using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseEnterAndExits : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event EventHandler OnMouseEnterEvent;
    public event EventHandler OnMouseExitEvent;

    //鼠标移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterEvent?.Invoke(this, EventArgs.Empty);
    }

    //鼠标移出
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitEvent?.Invoke(this, EventArgs.Empty);
    }


}
