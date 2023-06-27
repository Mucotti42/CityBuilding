using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarAI : MonoBehaviour
{
    private List<Transform> waypoints;
    [SerializeField] private Vector2Int startingCoord;

    private Transform _transform;
    private Vector2Int lastPosition;
    private int lastIndex = 0;
    public Vector2 offset;

    public Vector2 up;
    public Vector2 down;
    public Vector2 right;
    public Vector2 left;
    [SerializeField] private float lerp = .5f;

    private void Awake()
    {
        startingCoord = new Vector2Int(Random.Range(0, 11), Random.Range(0, 11));
        lastPosition = startingCoord;
        _transform = transform;
    }

    private void Start()
    {
        StartCoroutine(IEDrive());
    }

    private void GetInfo()
    {
        WaypointInfo info = TrafficSystem.instance.GetWaypoints(lastPosition);
        waypoints = info.waypoints;
        lastPosition = info.lastPositionCoord;
    }
    private IEnumerator IEDrive()
    {
        while (true)
        {
            GetInfo();
            for (int i = 0; i < waypoints.Count-1; i++)
            {
                CalculateRotationAndOffset(waypoints[i].position, waypoints[i + 1].position);
                CalculateLayer();
                for (int j = 0; j < 1000; j++)
                {
                    _transform.position = Vector3.Lerp(transform.position,Vector3.Lerp(waypoints[i].position, waypoints[i + 1].position, 0.001f * j) + (Vector3)offset,lerp);
                    yield return null;
                }                
            }
        }
    }

    private void CalculateRotationAndOffset(Vector3 from, Vector3 to)
    {
        Vector3 direction = to - from;
        int index;
        if (direction.x < 0)
        {
            if (direction.y < 0)
            {
                index = 0;
                offset = left;
            }
            else
            {
                index = 1;
                offset = up;
            }
        }
        else
        {
            if (direction.y < 0)
            {
                index = 2;
                offset = down;
            }
            else
            {
                index = 3;
                offset = right;
            }
        }
        transform.GetChild(lastIndex).gameObject.SetActive(false);
        transform.GetChild(index).gameObject.SetActive(true);
        lastIndex = index;
    }
    private void CalculateLayer()
    {
        
    }
}
