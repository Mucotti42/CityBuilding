using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        draggingBuilding.parent = GameObject.Find("Canvas").transform.GetChild(0);
        draggingBuilding.localScale = Vector3.one;
        StartCoroutine(IEDragging());
    }

    private IEnumerator IEDragging()
    {
        while (true)
        {
            Vector3 pos = Input.GetTouch(0).position;
            draggingBuilding.position = pos;
            MapController.instance.Preview(pos, selectedModel.tilling,draggingBuilding);
            yield return null;
        }
    }
    private void StopDragging()
    {
        Debug.LogWarning("DraggingS");
        if(!isSelected) return;

        Destroy(draggingBuilding.gameObject);
        StopAllCoroutines();

        if (MapController.instance.Preview(Input.GetTouch(0).position, selectedModel.tilling))
        {
            Debug.Log("Place" , MapController.instance.Fill(Input.GetTouch(0).position,selectedModel));
        }
        
        selectedModel = null;
        isSelected = false;
    }
}