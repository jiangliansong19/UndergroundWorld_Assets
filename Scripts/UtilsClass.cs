using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    //screen point convert to world point
    public static Vector3 GetCurrentWorldPoint()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0;
        return point;
    }

    public static Vector3 getRoundCurrentWorldPoint()
    {
        Vector3 point = GetCurrentWorldPoint();
        return new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), 0);
    }

    //sprite convert to Texture
    public static Texture GetTextureFromSprite(Sprite sprite, float scale = 1.0f)
    {
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(0, 0, (int)(sprite.textureRect.width * scale), (int)(sprite.textureRect.height * scale), pixels, 0);
        croppedTexture.Apply();
        return croppedTexture;
    }

    public static bool isTouchPointInScreenRect(Rect rect)
    {
        Vector3 point = Input.mousePosition;
        return point.x > rect.xMin && point.x < rect.xMax && point.y > rect.yMin && point.y < rect.yMax;
    }

    //rayCast
    public static GameObject GetObjectByRay(Vector3 atPosition)
    {
        RaycastHit2D obj = Physics2D.Raycast(new Vector2(atPosition.x, atPosition.y), Vector3.zero);
        if (obj.collider != null)
        {
            return obj.collider.gameObject;
        }
        return null;
    }

    public static GameObject[] GetObjectByRay2D(Vector2 start, Vector2 end)
    {
        RaycastHit2D[] objs = Physics2D.RaycastAll(start, end - start, (end - start).sqrMagnitude);
        if (objs != null && objs.Length > 0)
        {
            GameObject[] list = new GameObject[objs.Length];
            int index = 0;
            foreach (RaycastHit2D item in objs)
            {
                list[index] = item.collider.gameObject;
                index++;
            }
            return list;
        }
        return null;
    }

    public static string GetStringWithColor(string text, string color)
    {
        return "<color=" + color + ">" + text + "</color>";
    }
}
