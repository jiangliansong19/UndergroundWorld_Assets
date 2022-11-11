using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// TouchBegin, TouchMoving, TouchEnd
/// 附着在MainCamera上。
/// </summary>
public class TouchEventHandler : MonoBehaviour
{
    //关键字段，true表示开始工作
    public bool isWorking = false;

    public class TouchEventArgs
    {
        public Vector2 position;
    }

    //开始点
    public event EventHandler<TouchEventArgs> OnTouchBegin;

    //移动中
    public event EventHandler<TouchEventArgs> OnTouchMoving;

    //结束点
    public event EventHandler<TouchEventArgs> OnTouchEnd;


    //鼠标被按住的帧数
    private int mouseHoldFrame;

    //鼠标被按住的位置
    private Vector2 mouseHoldPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isWorking)
        {
            this.CheckTouchPositionChanged();
        }
    }


    private void CheckTouchPositionChanged()
    {
                //按下鼠标左键，此时进入画框状态，并确定框的起始点
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (mouseHoldFrame == 0)
            {
                this.OnMouseLeftTouchBegin();
            }

            //按住时间超过15帧+mousePositon有值+mousePosition!=Input.mousePosition
            if (mouseHoldFrame > 20 && 
                !Vector2.Equals(Vector2.zero, mouseHoldPosition) && 
                !Vector2.Equals(Input.mousePosition, mouseHoldPosition))
            {
                this.OnMouseLeftTouchMove();
            }

            mouseHoldFrame++;
            mouseHoldPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseHoldFrame = 0;
            mouseHoldPosition = Vector2.zero;

            this.OnMouseLeftTouchEnd();
        }
    }

    private void OnMouseLeftTouchBegin()
    {
        this.OnTouchBegin.Invoke(this, new TouchEventArgs { position = UtilsClass.GetCurrentWorldPoint() });
    }

    private void OnMouseLeftTouchMove()
    {
        this.OnTouchMoving.Invoke(this, new TouchEventArgs { position = UtilsClass.GetCurrentWorldPoint() });
    }

    private void OnMouseLeftTouchEnd()
    {
        this.OnTouchEnd.Invoke(this, new TouchEventArgs { position = UtilsClass.GetCurrentWorldPoint() });
    }
}
