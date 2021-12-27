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
            _tmpTransform.position = UtilsClass.GetCurrentWorldPoint();
        }
    }

    private void BuildingManager_OnActiveBuildingTypeChangedHandler1(object sender, BuildingManager.OnActiveBuildingTypeChangedHandlerArgs e)
    {
        if (e.Args_TypeSO != null)
        {
            Sprite sprite;
            if (e.Args_TypeSO.prefab == null)
            {
                sprite = e.Args_TypeSO.sprite;

            }
            else
            {
                sprite = e.Args_TypeSO.prefab.GetComponent<SpriteRenderer>().sprite;
            }

            spriteRenderer.sprite = sprite;
            spriteRenderer.color = new Color(1, 1, 1, 0.75f);

            Cursor.visible = false;
        }
        else
        {
            spriteRenderer.sprite = null;
            spriteRenderer.color = new Color(1, 1, 1, 1.0f);

            Cursor.visible = true;
        }
    }

    private void BuildingManager_OnActiveBuildingTypeChangedHandler(object sender, BuildingManager.OnActiveBuildingTypeChangedHandlerArgs e)
    {
        if (e.Args_TypeSO != null)
        {
            _tmpTransform = Instantiate(e.Args_TypeSO.prefab, UtilsClass.GetCurrentWorldPoint(), Quaternion.identity);
            Destroy(_tmpTransform.GetComponent<BoxCollider2D>());
        }
        else
        {
            Destroy(_tmpTransform.gameObject);
        }
    }
}
