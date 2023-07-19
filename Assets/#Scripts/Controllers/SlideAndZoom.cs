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
    private Transform scaleReferance;
    private Vector2 mapBorder = new Vector2(3214, 2950);
    
    float initialScale = 1;
    Vector3 initialPosition = Vector3.zero;
    Vector3 touchMidPoint = Vector3.zero;
    private void Awake()
    {
        _transform = transform;
        scaleReferance = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0);
    }

    [ContextMenu("ScreenInfo")]
    public void ScreenInfo()
    {
        Debug.Log(Screen.width / Screen.dpi+" "+Screen.height / Screen.dpi +" " + Screen.currentResolution);
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
                initialScale = _transform.localScale.x;
                initialPosition = _transform.position;
                touchMidPoint = (touch1.position + touch2.position) / 2;
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float pinchAmount = (currentDistance - initialDistance) * zoomSpeed;

                float scale = transform.localScale.x;
                scale += pinchAmount;
                scale = Mathf.Clamp(scale, 2, 10);
                transform.localScale = Vector3.one * scale;

                Vector3 touchDelta;
                if(scale > initialScale)
                    touchDelta = -Vector3.Lerp(initialPosition , touchMidPoint - initialPosition,(scale - initialScale) / (10f-initialScale));
                else
                    touchDelta = Vector3.Lerp(initialPosition , touchMidPoint - initialPosition,(scale - initialScale) / (initialScale-2));
                transform.position = initialPosition + touchDelta;
            }
            Clamp();
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
                    Clamp();
                    break;;
                }
            }
        }  
    }

    private void Clamp()
    {
        var scale = scaleReferance.lossyScale.x;
        float mapX = (scale * mapBorder.x) /2 - Screen.currentResolution.width/2;
        float mapY = (scale * mapBorder.y) /2 - Screen.currentResolution.height/2;
        float clampedX = Mathf.Clamp(_transform.localPosition.x, -mapX, mapX);
        float clampedY = Mathf.Clamp(_transform.localPosition.y, -mapY, mapY);
        _transform.localPosition = new Vector2(clampedX, clampedY);
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
