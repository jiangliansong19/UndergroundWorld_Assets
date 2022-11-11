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

    public Material GLRectMat;//��ͼ�Ĳ��ʣ���Inspector������

    public UnityEngine.Color GLRectColor;//���ε��ڲ���ɫ����Inspector������

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
            GL.PushMatrix(); //���浱ǰMatirx  
            GLRectMat.SetPass(0); //ˢ�µ�ǰ����  
            GL.LoadPixelMatrix();//����pixelMatrix  
            GL.Color(GLRectColor);
            GL.Begin(GL.LINES);
            GL.Vertex3(startPosition.x, startPosition.y, 0);
            GL.Vertex3(endPosition.x, endPosition.y, 0);
            GL.End();
            GL.PopMatrix();//��ȡ֮ǰ��Matrix  
        }
    }
}
