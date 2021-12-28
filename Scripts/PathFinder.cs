using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance {private set; get; }

    private Transform EmptyTransfrom = new GameObject().transform;

    private void Awake()
    {
        Instance = this;
    }

    //Judge whether a is connected to b
    public bool isConnectBetween(Transform a, Transform b)
    {
        Transform leftTrans = Physics2D.Raycast(a.position, Vector2.left, 1).collider.transform;
        Transform rightTrans = Physics2D.Raycast(a.position, Vector2.right, 1).collider.transform;
        if (leftTrans == b || rightTrans == b)
        {
            return true;
        }
        else
        {
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
        }
        return false;
    }




    private Transform CanWalkToLeft(Vector3 position)
    {
        RaycastHit2D pathObj = Physics2D.Raycast(position, Vector2.left, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }

        RaycastHit2D leftObj = Physics2D.Raycast(position, Vector2.left, 1);
        if (leftObj.collider == null)
        {
            leftObj = Physics2D.Raycast(new Vector2(position.x - 1, position.y - 1), Vector2.left, 1, LayerMask.GetMask("Soil"));
            if (leftObj.collider != null)
            {
                return leftObj.collider.transform;
            }
        }
        return null;
    }

    private Transform CanWalkToRight(Vector3 position)
    {
        RaycastHit2D pathObj = Physics2D.Raycast(position, Vector2.right, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }

        RaycastHit2D rightObj = Physics2D.Raycast(position, Vector2.right, 1);
        if (rightObj.collider == null)
        {
            rightObj = Physics2D.Raycast(new Vector2(position.x + 1, position.y - 1), Vector2.right, 1, LayerMask.GetMask("Soil"));
            if (rightObj.collider != null)
            {
                return rightObj.collider.transform;
            }
        }
        return null;
    }

    private Transform CanWalkToTop(Vector3 position)
    {
        RaycastHit2D pathObj = Physics2D.Raycast(position, Vector2.up, 1, LayerMask.GetMask("Path"));
        if (pathObj.collider != null)
        {
            return pathObj.collider.transform;
        }
        return null;
    }

    private Transform CanWalkToDown(Vector3 position)
    {
        RaycastHit2D pathObj = Physics2D.Raycast(position, Vector2.down, 1, LayerMask.GetMask("Path"));
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
