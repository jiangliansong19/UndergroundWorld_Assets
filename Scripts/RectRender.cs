using System;
using System.Drawing;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;


//UGUI系统上，的EventSystem提供了一些方法。
//那就是EventSystem.current.IsPointerOverGameObject（）方法，作用：判断鼠标是否点击在UI上。
//EventSystem.current.IsPointerOverGameObject()

/// <summary>
/// 绘制矩形框区域
/// </summary>
public class RectRender : MonoBehaviour
{
    private bool onDrawingRect;//是否正在画框(即鼠标左键处于按住的状态)

    private Vector3 startPoint;//框的起始点，即按下鼠标左键时指针的位置
    private Vector3 endPoint;//框的终止点，即放开鼠标左键时指针的位置

    private ToolTipsUI tipsUI;

    private void Awake()
    {
        TouchEventHandler handler = Camera.main.GetComponent<TouchEventHandler>();

        handler.OnTouchBegin += OnMouseLeftTouchBegin;
        handler.OnTouchMoving += OnMouseLeftTouchMove;
        handler.OnTouchEnd += OnMouseLeftTouchEnd;
    }

    private void Update()
    {

    }

    private void OnMouseLeftTouchBegin(object o, TouchEventHandler.TouchEventArgs e)
    {
        startPoint = new Vector2(Mathf.Round(e.position.x), Mathf.Round(e.position.y));

        ToolTipsUI.Instance.Hide();
    }

    private void OnMouseLeftTouchMove(object o, TouchEventHandler.TouchEventArgs e)
    {
        Vector2 currentPoint = new Vector2(Mathf.Round(e.position.x), Mathf.Round(e.position.y));

        Debug.LogFormat("开始画框，起点:{0}", startPoint);

        if (onDrawingRect == false)
        {
            endPoint = currentPoint;
            //开始进入绘制阶段
            onDrawingRect = true;
        }

        //如果终点坐标发生了变化
        if (!Vector2.Equals(currentPoint, endPoint))
        {
            endPoint = currentPoint;

            float witdh = Mathf.Abs(currentPoint.x - startPoint.x) + 1;
            float height = Mathf.Abs(currentPoint.y - startPoint.y) + 1;

            Debug.LogFormat("画框中，当前点:{0}", currentPoint);
            ToolTipsUI.Instance.Show(
                witdh + " x " + height,
                new ToolTipsUI.TooltipTimer { timer = 1000 },
                new ToolTipsUI.ToolTipPosition { position = UtilsClass.GetCurrentScreenPoint(startPoint) });
        }
    }

    private void OnMouseLeftTouchEnd(object o, TouchEventHandler.TouchEventArgs e)
    {

        onDrawingRect = false;

        ToolTipsUI.Instance.Hide();

        Debug.LogFormat("画框结束，终点:{0}", endPoint);

    }

    
    /// <summary>
    /// 计算矩形区域
    /// </summary>
    /// <param name="sPosition">起点</param>
    /// <param name="ePosition">终点</param>
    /// <returns></returns>
    Vector2[] GetRectangleCoordinate(Vector2 sPosition, Vector2 ePosition)
    {
        float sx, sy, ex, ey;
        if (ePosition.x > sPosition.x)
        {
            sx = sPosition.x - 0.5f;
            ex = ePosition.x + 0.5f;
        }
        else
        {
            sx = sPosition.x + 0.5f;
            ex = ePosition.x - 0.5f;
        }

        if (ePosition.y > sPosition.y)
        {
            sy = sPosition.y - 0.5f;
            ey = ePosition.y + 0.5f;
        }
        else
        {
            sy = sPosition.y + 0.5f;
            ey = ePosition.y - 0.5f;
        }

        return new Vector2[] { new Vector2(sx, sy), new Vector2(ex, ey) };
    }
    


    public Material GLRectMat;//绘图的材质，在Inspector中设置
    public UnityEngine.Color GLRectColor;//矩形的内部颜色，在Inspector中设置
    public UnityEngine.Color GLRectEdgeColor;//矩形的边框颜色，在Inspector中设置

    private float z = 0.1f;

    void OnPostRender()
    {
        if (onDrawingRect)
        {

            Vector2[] vectors = GetRectangleCoordinate(startPoint, endPoint);

            Vector2 sPoint = Camera.main.WorldToScreenPoint(vectors[0]);
            Vector2 cPoint = Camera.main.WorldToScreenPoint(vectors[1]);

            //准备工作:获取确定矩形框各角坐标所需的各个数值
            float Xmin = Mathf.Min(sPoint.x, cPoint.x);
            float Xmax = Mathf.Max(sPoint.x, cPoint.x);
            float Ymin = Mathf.Min(sPoint.y, cPoint.y);
            float Ymax = Mathf.Max(sPoint.y, cPoint.y);

            //开始画图

            GL.PushMatrix();//GL入栈
                            //在这里，只需要知道GL.PushMatrix()和GL.PopMatrix()分别是画图的开始和结束信号,画图指令写在它们中间
            if (!GLRectMat)
                return;

            GLRectMat.SetPass(0);//启用线框材质rectMat

            GL.LoadPixelMatrix();//设置用屏幕坐标绘图


            /*------第一步，绘制矩形------*/
            GL.Begin(GL.QUADS);//开始绘制矩形,这一段画的是框中间的半透明部分，不包括边界线

            GL.Color(GLRectColor);//设置矩形的颜色，注意GLRectColor务必设置为半透明

            //陈述矩形的四个顶点
            GL.Vertex3(Xmin, Ymin, z);//陈述第一个点，即框的左下角点，记为点1
            GL.Vertex3(Xmin, Ymax, z);//陈述第二个点，即框的左上角点，记为点2
            GL.Vertex3(Xmax, Ymax, z);//陈述第三个点，即框的右上角点，记为点3
            GL.Vertex3(Xmax, Ymin, z);//陈述第四个点，即框的右下角点，记为点4

            GL.End();//告一段落，此时画好了一个无边框的矩形


            /*------第二步，绘制矩形的边框------*/
            GL.Begin(GL.LINES);//开始绘制线，用来描出矩形的边框

            GL.Color(GLRectEdgeColor);//设置方框的边框颜色，建议设置为不透明的

            //描第一条边
            GL.Vertex3(Xmin, Ymin, z);//起始于点1
            GL.Vertex3(Xmin, Ymax, z);//终止于点2

            //描第二条边
            GL.Vertex3(Xmin, Ymax, z);//起始于点2
            GL.Vertex3(Xmax, Ymax, z);//终止于点3

            //描第三条边
            GL.Vertex3(Xmax, Ymax, z);//起始于点3
            GL.Vertex3(Xmax, Ymin, z);//终止于点4

            //描第四条边
            GL.Vertex3(Xmax, Ymin, z);//起始于点4
            GL.Vertex3(Xmin, Ymin, z);//返回到点1

            GL.End();//画好啦！

            GL.PopMatrix();//GL出栈

        }
    }

}