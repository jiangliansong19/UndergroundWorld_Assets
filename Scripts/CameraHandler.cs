using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


/// <summary>
/// 镜头的跟随与缩放
/// </summary>
public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance { private set; get; }

    //虚拟相机
    [SerializeField] private CinemachineVirtualCamera cinemachineVirturalCamera;

    private float orthographicSize;
    private float targetOrthographicSize;


    private bool _handleMouseEnable = true;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        orthographicSize = cinemachineVirturalCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (_handleMouseEnable == true)
        {
            HandleMouseMoved();
            HandleMouseZoomed();
        }

    }

    //镜头跟随
    private void HandleMouseMoved()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;

        //移动速度
        float moveSpeed = 30f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    //镜头缩放
    private void HandleMouseZoomed()
    {
        float zoomAmount = 2f;
        targetOrthographicSize += Input.mouseScrollDelta.y * zoomAmount;

        float minOrthoGraphicsSize = 5;
        float maxOtheoGraphicsSIze = 15;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthoGraphicsSize, maxOtheoGraphicsSIze);

        //缩放速度
        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirturalCamera.m_Lens.OrthographicSize = orthographicSize;
    }


    //==========================================================================
    //==========================================================================


    public void SetHandleMouseEnable(bool enable)
    {
        _handleMouseEnable = enable;
    }
}
