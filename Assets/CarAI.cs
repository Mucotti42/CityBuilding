using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarAI : MonoBehaviour
{
    private List<Transform> waypoints;
    private List<Vector2Int> waypointCoords;
    [SerializeField] private Vector2Int startingCoord;

    private Transform _transform;
    private Vector2Int lastPosition;
    private int lastIndex = 0;
    public Vector2 offset;

    private Vector2 up = new Vector2(3.56f,2);
    private Vector2 down = new Vector2(-3.56f,-6.3f);
    private Vector2 right = new Vector2(5.95f,-8.57f);
    private Vector2 left = new Vector2(1.7f, -7.93f);

    private Transform scaleReferance;
    private void Awake()
    {
        startingCoord = new Vector2Int(Random.Range(0, 11), Random.Range(0, 11));
        lastPosition = startingCoord;
        _transform = transform;
        scaleReferance = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0);
    }

    private void Start()
    {
        StartCoroutine(IEDrive());
    }

    private void GetInfo()
    {
        WaypointInfo info = TrafficSystem.instance.GetWaypoints(lastPosition);
        waypoints = info.waypoints;
        waypointCoords = info.waypointCoords;
        lastPosition = waypointCoords[^1];
    }
    private IEnumerator IEDrive()
    {
        while (true)
        {
            GetInfo();
            for (int i = 0; i < waypoints.Count-1; i++)
            {
                CalculateRotationOffsetLayer(waypoints[i].position, waypoints[i + 1].position, i);
                for (int j = 0; j < 1000; j++)
                {
                    _transform.position = Vector3.Lerp(transform.position,Vector3.Lerp(waypoints[i].position, waypoints[i + 1].position, 0.001f * j) + ((Vector3)offset * Mathf.Sqrt(scaleReferance.lossyScale.x)),.5f);
                    yield return null;
                }                
            }
        }
    }

    private void CalculateRotationOffsetLayer(Vector3 from, Vector3 to,int waypoint)
    {
        Vector3 direction = to - from;
        int index;
        if (direction.x < 0)
        {
            if (direction.y < 0)
            {
                index = 0;
                offset = left;
                CalculateLayer(waypointCoords[waypoint]);
            }
            else
            {
                index = 1;
                offset = up;
                CalculateLayer(waypointCoords[waypoint+1]);
            }
        }
        else
        {
            if (direction.y < 0)
            {
                index = 2;
                offset = down;
                CalculateLayer(waypointCoords[waypoint]);
            }
            else
            {
                index = 3;
                offset = right;
                CalculateLayer(waypointCoords[waypoint+1]);
            }
        }
        transform.GetChild(lastIndex).gameObject.SetActive(false);
        transform.GetChild(index).gameObject.SetActive(true);
        lastIndex = index;
    }
    private void CalculateLayer(Vector2Int coord)
    {
        if(coord.x == 10 || coord.y == 10) return;
        
        _transform.SetParent(MapController.instance.GetTile(coord));
        _transform.SetSiblingIndex(0);
        
    }
}
