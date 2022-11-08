using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMouse : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Transform _tmpTransform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChangedHandler += BuildingManager_OnActiveBuildingTypeChangedHandler;
    }

    private void Update()
    {
        //transform.position = UtilsClass.GetCurrentWorldPoint();
        if (_tmpTransform != null)
        {
            //���Ͻ�Ϊԭ�㣬��ÿһ��prefab��position��ʾ�����ĵ㡣
            //������  -0.5 < x < 0.5, -0.5 < y < 0.5 ʱ��prefab��λ�ö��� vector2(0,0)
            Vector2 point = UtilsClass.GetCurrentWorldPoint();
            _tmpTransform.position = new Vector2(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
        }
    }

    // ���뽨���༭״̬
    private void BuildingManager_OnActiveBuildingTypeChangedHandler(object sender, BuildingManager.OnActiveBuildingTypeChangedHandlerArgs e)
    {
        if (e.Args_TypeSO != null)
        {
            _tmpTransform = Instantiate(e.Args_TypeSO.prefab, UtilsClass.GetCurrentWorldPoint(), Quaternion.identity);
            Destroy(_tmpTransform.GetComponent<BoxCollider2D>());
        }
        else
        {
            if (_tmpTransform != null)
            {
                Destroy(_tmpTransform.gameObject);
            }
        }
    }
}
