using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance {private set; get; }

    private Transform EmptyTransfrom;

    private void Awake()
    {
        Instance = this;
        EmptyTransfrom = new GameObject().transform;
    }

    //Judge whether a is connected to b
    public bool isConnectBetween(Transform a, Transform b)
    {
        Vector3 leftPosition = UtilsClass.GetWorldPoint(new Vector3(a.position.x - 1, a.position.y, 0));
        RaycastHit2D[] leftHits = Physics2D.RaycastAll(leftPosition, Vector3.zero);
        foreach(RaycastHit2D hit in leftHits)
        {
            if (hit.collider == b)
            {
                return true;
            }
        }

        Vector3 rightPosition = UtilsClass.GetWorldPoint(new Vector3(a.position.x + 1, a.position.y, 0));
        RaycastHit2D[] rightHits = Physics2D.RaycastAll(rightPosition, Vector3.zero);
        foreach (RaycastHit2D hit in rightHits)
        {
            if (hit.collider == b)
            {
                return true;
            }
        }

        Transform top = CanWalkToTop(a.position);
        while (top)
        {
            isConnectBetween(top, b);
        }

        Transform right = CanWalkToRight(a.position);
        while (right)
        {
            isConnectBetween(right, b);
        }

        Transform left = CanWalkToLeft(a.position);
        while (left)
        {
            isConnectBetween(left, b);
        }

        Transform down = CanWalkToDown(a.position);
        while (down)
        {
            isConnectBetween(down, b);
        }

        return false;
    }




    private Transform CanWalkToLeft(Vector3 position)
    {
        Vector3 leftPosition = new Vector3(position.x - 1, position.y, 0);
        Vector3 leftDownPosition = new Vector3(position.x - 1, position.y - 1, 0);

        RaycastHit2D test = Physics2D.Raycast(leftPosition, Vector2.zero);
        GameObject testobj = UtilsClass.GetObjectByRay(leftPosition);
        GameObject testobj1 = UtilsClass.GetObjectByRay(UtilsClass.GetWorldPoint(leftPosition));
        RaycastHit2D pathObj = Physics2D.Raycast(leftPosition, Vector2.zero, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }
        else
        {
            RaycastHit2D leftObj = Physics2D.Raycast(leftPosition, Vector2.zero, 1);
            if (leftObj.collider == null)
            {
                leftObj = Physics2D.Raycast(leftDownPosition, Vector2.zero, 1, LayerMask.GetMask("Soil"));
                if (leftObj.collider != null)
                {
                    return leftObj.collider.transform;
                }
            }
        }

        return null;
    }

    private Transform CanWalkToRight(Vector3 position)
    {
        Vector3 rightPosition = UtilsClass.GetWorldPoint(new Vector3(position.x + 1, position.y, 0));
        Vector3 rightDownPosition = UtilsClass.GetWorldPoint(new Vector3(position.x + 1, position.y - 1, 0));

        RaycastHit2D pathObj = Physics2D.Raycast(rightPosition, Vector2.zero, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }
        else
        {
            RaycastHit2D rightObj = Physics2D.Raycast(position, Vector2.zero, 1);
            if (rightObj.collider == null)
            {
                rightObj = Physics2D.Raycast(rightDownPosition, Vector2.zero, 1, LayerMask.GetMask("Soil"));
                if (rightObj.collider != null)
                {
                    return rightObj.collider.transform;
                }
            }
        }
        return null;
    }

    private Transform CanWalkToTop(Vector3 position)
    {
        Vector3 upPosition = UtilsClass.GetWorldPoint(new Vector3(position.x, position.y + 1, 0));
        RaycastHit2D pathObj = Physics2D.Raycast(upPosition, Vector2.zero, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }
        return null;
    }

    private Transform CanWalkToDown(Vector3 position)
    {
        Vector3 downPosition = UtilsClass.GetWorldPoint(new Vector3(position.x, position.y - 1, 0));
        RaycastHit2D pathObj = Physics2D.Raycast(downPosition, Vector2.zero, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }
        return null;
    }



    private Transform[] RayCastTransforms(Vector3 atPosition)
    {
        Transform up = CastRayTop(atPosition) ?? EmptyTransfrom;
        Transform left = CastRayLeft(atPosition) ?? EmptyTransfrom;
        Transform right = CastRayRight(atPosition) ?? EmptyTransfrom;
        Transform down = CastRayBottom(atPosition) ?? EmptyTransfrom;
        return new Transform[4] { up, right, down, left };
    }

    private Transform CastRayTop(Vector3 position)
    {
        RaycastHit2D obj = Physics2D.Raycast(position, Vector2.up, 1, LayerMask.GetMask("Path"));
        return obj.collider.transform;
    }

    private Transform CastRayLeft(Vector3 position)
    {
        RaycastHit2D obj = Physics2D.Raycast(position, Vector2.left, 1);
        return obj.collider.transform;
    }

    private Transform CastRayRight(Vector3 position)
    {
        RaycastHit2D obj = Physics2D.Raycast(position, Vector2.right, 1);
        return obj.collider.transform;
    }

    private Transform CastRayBottom(Vector3 position)
    {
        RaycastHit2D obj = Physics2D.Raycast(position, Vector2.down, LayerMask.GetMask("Path"));
        return obj.collider.transform;
    }
}
