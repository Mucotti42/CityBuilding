using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DraggingController : MonoBehaviour
{
    public static DraggingController instance;
    [SerializeField] private Transform topLayer;
    
    public bool isDragging = false;
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

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Update()
    {
        if (Input.touchCount < 1) return;
        if(Input.GetTouch(0).phase != TouchPhase.Ended) return;
        
        StopDragging();
    }

    private void StartDragging(BuildingModel building)
    {
        selectedModel = building;
        isDragging = true;

        draggingBuilding = Instantiate(selectedModel.prefab).transform;
        draggingBuilding.SetParent(topLayer);
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
        if(!isDragging) return;

        Destroy(draggingBuilding.gameObject);
        StopAllCoroutines();

        if (MapController.instance.Preview(Input.GetTouch(0).position, selectedModel.tilling))
        {
            MapController.instance.Fill(Input.GetTouch(0).position, selectedModel);
        }
        
        selectedModel = null;
        isDragging = false;
    }
}