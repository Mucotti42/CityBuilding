using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideAndZoom : MonoBehaviour
{
    public float zoomSpeed = 0.1f;
    public float moveSpeed = 1f;

    private Camera mainCamera;
    private float initialCameraSize;
    private Vector3 initialTouchPosition;
    private Transform _transform;

    private float initialDistance;
    private float distance;
    private bool slide = false;

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

        if (Input.touchCount == 1)
        {
            TouchPhase phase = Input.GetTouch(0).phase;
            switch (phase)
            {
                case TouchPhase.Began:
                {
                    if(IsTouchOverUI()) return;
                    
                    slide = true;    
                    break;
                }
                case TouchPhase.Ended:
                {
                    slide = false;
                    break;
                }
                case TouchPhase.Moved:
                {
                    if(IsTouchOverUI() || !slide) return;
                    
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    Vector3 moveDirection = new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0f);
                    _transform.position -= moveDirection * -moveSpeed * Time.deltaTime;
                    
                    break;;
                }
            }
        }  
    }
    
    
    private bool IsTouchOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.GetTouch(0).position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        for (int i = 0; i < results.Count; i++)
        {
            if(results[i].gameObject.layer == 2)
                return true;
        }

        return false;
    }
}
