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


    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitEvent?.Invoke(this, EventArgs.Empty);
    }
}
