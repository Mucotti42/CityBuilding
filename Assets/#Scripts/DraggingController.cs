using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DraggingController : MonoBehaviour
{
    private bool isSelected = false;
    private BuildingModel selectedModel;
    private Transform draggingBuilding;
    private Transform draggingBuildingPreview;

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
        draggingBuildingPreview = Instantiate(selectedModel.prefab).transform;
        draggingBuilding.parent = GameObject.Find("Canvas").transform;
        draggingBuildingPreview.parent = GameObject.Find("Canvas").transform;
        draggingBuildingPreview.GetChild(0).GetComponent<Image>().color =
            new Color(0.6396768f, 1f, 0f, 0.454902f);
        StartCoroutine(IEDragging());
    }

    private IEnumerator IEDragging()
    {
        while (true)
        {
            Vector3 pos = Input.GetTouch(0).position;
            draggingBuilding.position = pos;
            MapController.instance.Preview(pos, selectedModel.tilling,draggingBuildingPreview);
            //draggingBuildingPreview.position = new Vector3(-1000, -1000, -1000);
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