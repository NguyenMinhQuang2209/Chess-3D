using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance;
    [SerializeField] private LayerMask touchMask;
    [SerializeField] private Transform cameraLook = null;
    [SerializeField] private bool isUseMoveCamera = false;
    private TouchItem currentItem = null;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentItem?.OnTouchOutAction();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, touchMask);
            if (isHit)
            {
                if (hit.collider.gameObject.TryGetComponent<TouchItem>(out currentItem))
                {
                    currentItem.OnTouchAction();
                    MoveCamera(currentItem.transform.position);
                }
            }
            else
            {
                MoveCamera(new(0, 0, 0));
            }
        }

    }
    private void MoveCamera(Vector3 newPos)
    {
        if (!isUseMoveCamera) return;
        cameraLook.transform.position = newPos;
    }
}
