using System;
using System.Collections;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    public GameObject road;

    public Vector2Int coord;
    private RoadManager.RoadTypes type;

    private void Awake()
    {
        coord.x = transform.GetSiblingIndex() % 54;
        coord.y = transform.GetSiblingIndex() / 54;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(MapController.instance.GetTileContent(coord.x,coord.y) == TileContent.Empty)
            type = RoadManager.instance.RoadCreation(coord);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // RoadManager.instance.RoadDestruction(coord);
            // DestroyRoad();
        }
    }

    public void DestroyRoad()
    {
        Destroy(transform.GetChild(1).gameObject);
    }
    public void EditRoadPosition(Transform roadTransform)
    {
        roadTransform.parent = transform;
        roadTransform.localPosition = Vector3.zero;
    }
}