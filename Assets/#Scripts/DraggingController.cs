using System;
using System.Collections;
using UnityEngine;

public class DraggingController : MonoBehaviour
{
    private bool isSelected = false;
    private BuildingModel selectedModel;
    private Transform draggingBuilding;

    #region Events

    private void Start()
    {
        EventManager.instance.OnStartDragging += StartDragging;
    }

    private void OnDisable()
    {
        EventManager.instance.OnStartDragging -= StartDragging;
    }

    #endregion

    private void Update()
    {
        if (Input.touchCount < 1) return;
        if(Input.GetTouch(0).phase != TouchPhase.Ended) return;
        
        StopDragging();
    }

    private void StartDragging(BuildingModel building)
    {
        Debug.LogWarning("Dragging");
        selectedModel = building;
        isSelected = true;

        draggingBuilding = Instantiate(selectedModel.prefab).transform;
        draggingBuilding.parent = GameObject.Find("Canvas").transform;
        StartCoroutine(IEDragging());
    }

    private IEnumerator IEDragging()
    {
        while (true)
        {
            draggingBuilding.position = Input.GetTouch(0).position;
            yield return null;
        }
    }
    private void StopDragging()
    {
        Debug.LogWarning("DraggingS");
        if(!isSelected) return;
        
        selectedModel = null;
        isSelected = false;
        
        Destroy(draggingBuilding.gameObject);
        StopAllCoroutines();
    }
}