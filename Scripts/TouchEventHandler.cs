using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// TouchBegin, TouchMoving, TouchEnd
/// ������MainCamera�ϡ�
/// </summary>
public class TouchEventHandler : MonoBehaviour
{
    //�ؼ��ֶΣ�true��ʾ��ʼ����
    public bool isWorking = false;

    public class TouchEventArgs
    {
        public Vector2 position;
    }

    //��ʼ��
    public event EventHandler<TouchEventArgs> OnTouchBegin;

    //�ƶ���
    public event EventHandler<TouchEventArgs> OnTouchMoving;

    //������
    public event EventHandler<TouchEventArgs> OnTouchEnd;


    //��걻��ס��֡��
    private int mouseHoldFrame;

    //��걻��ס��λ��
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
                //��������������ʱ���뻭��״̬����ȷ�������ʼ��
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (mouseHoldFrame == 0)
            {
                this.OnMouseLeftTouchBegin();
            }

            //��סʱ�䳬��15֡+mousePositon��ֵ+mousePosition!=Input.mousePosition
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
