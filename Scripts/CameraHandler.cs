using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirturalCamera;

    private float orthographicSize;
    private float targetOrthographicSize;

    // Start is called before the first frame update
    void Start()
    {
        orthographicSize = cinemachineVirturalCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ExploreTeamManager.Instance.isShowing)
        {
            HandleMouseMoved();
            HandleMouseZoomed();
        }

    }

    private void HandleMouseMoved()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleMouseZoomed()
    {
        float zoomAmount = 2f;
        targetOrthographicSize += Input.mouseScrollDelta.y * zoomAmount;

        float minOrthoGraphicsSize = 5;
        float maxOtheoGraphicsSIze = 15;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthoGraphicsSize, maxOtheoGraphicsSIze);

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirturalCamera.m_Lens.OrthographicSize = orthographicSize;
    }
}
