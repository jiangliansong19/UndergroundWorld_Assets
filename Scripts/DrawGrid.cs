using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.GlobalIllumination;


/// <summary>
/// �������������
/// </summary>
public class DrawGrid : MonoBehaviour
{
    public bool isShowGrid = false;

    /// <summary>
    /// �ߵĲ���
    /// </summary>
    public Material lineMat;

    /// <summary>
    /// �ߵ���ɫ
    /// </summary>
    public Color lineColor = Color.blue;

    /// <summary>
    /// �����ڵķ������ɫ
    /// </summary>
    public Color mouseOverColor;

    private Camera _camera;

    void Start()
    {
        if (lineColor == null)
        {
            lineColor = new Color(1, 1, 1, 0.5f);
        }
        if (mouseOverColor == null)
        {
            mouseOverColor = new Color(0, 1, 0, 1);
        }

        _camera = GetComponent<Camera>();
    }

    void OnPostRender()
    {
        if (isShowGrid)
        {
            DrawMeshGrid();
            DrawMouseOver();
        }

    }

    /// <summary>
    /// ��һ����������ת��Ϊ���ӵ�����
    /// </summary>
    /// <param name="pos"></param>
    public static Vector2 WorldPosToCellPos(Vector2 pos)
    {
        int x = (int)(pos.x > 0 ? pos.x + 0.5 : pos.x - 0.5);
        int y = (int)(pos.y > 0 ? pos.y + 0.5 : pos.y - 0.5);
        return new Vector2(x, y);
    }

    /// <summary>
    /// �õ�������ڵĸ��ӵ�����
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetMouseCellPos()
    {
        return WorldPosToCellPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }


    /// <summary>
    /// ��������
    /// </summary>
    void DrawMeshGrid()
    {
        if (lineMat == null)
        {
            throw new Exception("lineMat is null");
        }
        Vector2 cameraPos = _camera.transform.position;

        //����Ŀ��
        int width = _camera.pixelWidth;
        int height = _camera.pixelHeight;

        //�ӿ�������ռ���ĸ���
        Vector2 leftUp = _camera.ScreenToWorldPoint(new Vector2(0, height));
        Vector2 rightUp = _camera.ScreenToWorldPoint(new Vector2(width, height));
        Vector2 leftDown = _camera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 rightDown = _camera.ScreenToWorldPoint(new Vector3(width, 0));

        //����ӿ������������еĿ��
        float viewWidth = rightUp.x - leftDown.x;
        float viewHeight = rightUp.y - leftDown.y;

        //cell�ں���������ϵĸ���
        int x = (int)viewWidth + 1;
        int y = (int)viewHeight + 1;

        //x y��Ϊ����
        x = x / 2 * 2 + 1;
        y = y / 2 * 2 + 1;

        //�м�ķ���λ��
        float centerX = x / 2 + 1;
        float centerY = y / 2 + 1;

        //ƫ��
        float offsetX = cameraPos.x % 1;
        float offsetY = cameraPos.y % 1;

        //����
        for (int i = 0; i <= x; i++)
        {
            //��ʼλ��
            float posX = i - centerX + 0.5f;
            //�߸���������ƶ�
            posX += cameraPos.x;
            //����ƫ��
            posX -= offsetX;

            DrawLine(new Vector2(posX, leftUp.y), new Vector2(posX, leftDown.y), lineColor);
        }
        //����
        for (int j = 0; j <= y; j++)
        {
            //�߸���������ƶ�
            float posY = j - centerY + 0.5f;
            //�߸���������ƶ�
            posY += cameraPos.y;
            //����ƫ��
            posY -= offsetY;

            DrawLine(new Vector2(leftUp.x, posY), new Vector2(rightUp.x, posY), lineColor);
        }
    }

    /// <summary>
    /// ����������ĳ�������ϵ�Ч��
    /// </summary>
    void DrawMouseOver()
    {
        DrawSquare(GetMouseCellPos(), mouseOverColor);
    }

    /// <summary>
    /// ��һ���� ��������
    /// </summary>
    /// <param name="posA"></param>
    /// <param name="posB"></param>
    /// /// <param name="color"></param>
    void DrawLine(Vector2 posA, Vector2 posB, Color color)
    {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(color);
        GL.Vertex3(posA.x, posA.y, 0);
        GL.Vertex3(posB.x, posB.y, 0);
        GL.End();
    }

    /// <summary>
    /// ��һ������ ��������
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    void DrawSquare(Vector2 center, float width, float height, Color color)
    {
        Vector2 leftUp = new Vector2(center.x - width / 2f, center.y + height / 2f);
        Vector2 rightUp = new Vector2(center.x + width / 2f, center.y + height / 2f);
        Vector2 leftDown = new Vector2(center.x - width / 2f, center.y - height / 2f);
        Vector2 rightDown = new Vector2(center.x + width / 2f, center.y - height / 2f);

        DrawLine(rightUp, leftUp, color);
        DrawLine(leftUp, leftDown, color);
        DrawLine(leftDown, rightDown, color);
        DrawLine(rightDown, rightUp, color);
    }

    /// <summary>
    /// ��һ�� 1x1 ������ ��������
    /// </summary>
    /// <param name="center"></param>
    /// <param name="color"></param>
    void DrawSquare(Vector2 center, Color color)
    {
        DrawSquare(center, 1, 1, color);
    }
}
