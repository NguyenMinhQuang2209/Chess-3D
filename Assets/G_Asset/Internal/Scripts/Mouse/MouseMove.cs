using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    [Tooltip("X for min distance when scroll,Y for max distance when scroll")]
    [SerializeField] private Vector2 mouseScroll = new(5f, 20f);
    [SerializeField] private float scrollRate = 1f;
    [SerializeField] private float currentScrollValue = 5f;

    Vector3 dragXPos;

    [SerializeField] private float mouseSensitivity = 100f;
    float rotateXAxis = 0f;
    float rotateYAxis = 0f;

    private void Update()
    {


        if (Input.GetMouseButtonDown(2))
        {
            dragXPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            PressingMiddleMouse();
        }
        MouseScroll();

        mainCamera.transform.rotation = Quaternion.Euler(rotateXAxis, rotateYAxis, 0f);
    }
    public void MouseScroll()
    {
        bool isChanging = false;
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0)
        {
            currentScrollValue -= Time.deltaTime * scrollRate;
            currentScrollValue = Mathf.Max(currentScrollValue, mouseScroll.x);
            isChanging = true;
        }
        else if (scrollValue < 0)
        {
            currentScrollValue += Time.deltaTime * scrollRate;
            currentScrollValue = Mathf.Min(currentScrollValue, mouseScroll.y);
            isChanging = true;

        }

        if (isChanging)
        {
            var transposer = mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (transposer != null)
            {
                transposer.m_CameraDistance = currentScrollValue;
            }
        }
    }
    public void PressingMiddleMouse()
    {
        int width = Screen.width;
        int height = Screen.height;
        Vector3 differ = dragXPos - Input.mousePosition;
        float differRateX = (differ.y / height) * mouseSensitivity;
        float differRateY = (differ.x / width) * mouseSensitivity;
        rotateXAxis += differRateX;
        rotateYAxis -= differRateY;
    }
}
