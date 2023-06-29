using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAndZoom : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public float moveSpeed = 1f;

    private Camera mainCamera;
    private float initialCameraSize;
    private Vector3 initialTouchPosition;
    private Transform _transform;

    public float pa;
    public float dp;

    private float initialDistance;
    private float distance;
    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float pinchAmount = (currentDistance - initialDistance) * zoomSpeed;

                float scale = transform.localScale.x;
                scale += pinchAmount;
                scale = Mathf.Clamp(scale, 2, 10);
                transform.localScale = Vector3.one * scale;
            }
        }

        // Kaydırma kontrolü
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            initialTouchPosition = Input.GetTouch(0).position;
        }
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved && !DraggingController.instance.isDragging)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            Vector3 moveDirection = new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f);
            _transform.position -= moveDirection * -moveSpeed * Time.deltaTime;
        }
    }
}
