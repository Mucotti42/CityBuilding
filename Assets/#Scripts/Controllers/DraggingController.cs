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
    private SpriteRenderer buildingSprite;
    private Vector3 lastPosition;

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
        if(!isDragging) return;
        if (Input.touchCount < 1) return;
        if(Input.GetTouch(0).phase != TouchPhase.Ended) return;
        
        StopDragging();
    }

    private void StartDragging(BuildingModel building)
    {
        selectedModel = building;
        isDragging = true;

        draggingBuilding = Instantiate(selectedModel.prefab).transform;
        buildingSprite = draggingBuilding.transform.GetChild(1).GetComponent<SpriteRenderer>();
        //draggingBuilding.SetParent(topLayer);
        //draggingBuilding.localScale = Vector3.one;
        StartCoroutine(IEDragging());
    }

    private IEnumerator IEDragging()
    {
        while (true)
        {
            // Vector3 pos = Input.GetTouch(0).position;
            //draggingBuilding.position = pos;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                lastPosition = hit.point;
                draggingBuilding.position = lastPosition;
                MapController.instance.Preview(lastPosition, selectedModel.tilling,draggingBuilding,buildingSprite);
            }
            yield return null;
        }
    }
    private void StopDragging()
    {
        Destroy(draggingBuilding.gameObject);
        StopAllCoroutines();

        if (MapController.instance.Preview(lastPosition, selectedModel.tilling))
        {
            MapController.instance.Fill(lastPosition, selectedModel);
        }
        
        selectedModel = null;
        isDragging = false;
    }
}