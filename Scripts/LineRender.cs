using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    public void drawArraw(Vector2 startPosition, Vector2 endPosition, Color color)
    {
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        this.GLRectColor = color;
        this.onDrawingLine = true;
    }

    private bool onDrawingLine;

    private Vector2 startPosition;

    private Vector2 endPosition;

    public Material GLRectMat;//绘图的材质，在Inspector中设置

    public UnityEngine.Color GLRectColor;//矩形的内部颜色，在Inspector中设置

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnPostRender()
    {
        if (true)
        {
            if (!GLRectMat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
            GL.PushMatrix(); //保存当前Matirx  
            GLRectMat.SetPass(0); //刷新当前材质  
            GL.LoadPixelMatrix();//设置pixelMatrix  
            GL.Color(GLRectColor);
            GL.Begin(GL.LINES);
            GL.Vertex3(startPosition.x, startPosition.y, 0);
            GL.Vertex3(endPosition.x, endPosition.y, 0);
            GL.End();
            GL.PopMatrix();//读取之前的Matrix  
        }
    }
}
