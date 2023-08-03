using System;
using System.Collections;
using UnityEngine;

public class TileTest : MonoBehaviour
{
    public GameObject road;

    public Vector2Int coord;
    private RoadManager.RoadTypes type;

    private void Start()
    {
        CalculateCoord();
    }

    public void CalculateCoord()
    {
        var row = MapController.instance.columnRow.x;
        coord.x = transform.GetSiblingIndex() % row;
        coord.y = transform.GetSiblingIndex() / row;
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
        roadTransform.localScale = Vector3.one;
        roadTransform.GetComponentInChildren<SpriteRenderer>().sortingOrder = coord.x * coord.y + 3;
    }
}