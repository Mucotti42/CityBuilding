using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public struct WaypointInfo
{
    public List<Transform> waypoints;
    public List<Vector2Int> waypointCoords;

    public WaypointInfo(List<Transform> waypoints, List<Vector2Int> waypointCoords)
    {
        this.waypoints = waypoints;
        this.waypointCoords = waypointCoords;
    }
}
public class TrafficSystem : MonoBehaviour
{
    public static TrafficSystem instance;
    
    public bool drawGizmo = true;

    private Transform[,] waypointGrid = new Transform[11, 11];
    public List<Vector3> _waypoints = new List<Vector3>();

    private void Awake()
    {
        if (!instance) instance = this;
        
        InitilizeWaypoints();
        //_waypoints = GetWaypoints(new Vector2Int(5,5)).positions;
    }

    private void OnDrawGizmos()
    {
        if(!drawGizmo) return;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (_waypoints.Contains(transform.GetChild(i).position))
            {
                Gizmos.color = Color.blue;
            }
            else
                Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.GetChild(i).position,10f);
        }
   
        Gizmos.color = Color.yellow;
        for (int i = 0; i < _waypoints.Count-1; i++)
        {
            Vector3 lerpedPos = Vector3.Lerp(_waypoints[i], _waypoints[i + 1], .8f); 
            Gizmos.DrawLine(_waypoints[i], lerpedPos);
        }
    }

    private void InitilizeWaypoints()
    { 
        int index = 0;
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                waypointGrid[i, j] = transform.GetChild(index);
                index++;
            }
        }
    }

    public WaypointInfo GetWaypoints(Vector2Int startingCoord)
    {
        bool control = true;
        
        List<Transform> waypoints = new List<Transform>();
        List<Vector2Int> indexs = new List<Vector2Int>();
        
        waypoints.Add(waypointGrid[startingCoord.x,startingCoord.y]);
        indexs.Add(startingCoord);
        
        for (int i = 1; i < 100;)
        {
            Vector2Int lastPos = indexs[indexs.Count - 1];
            Vector2Int newPos = new Vector2Int();
            List<Vector2Int> possibilities = new List<Vector2Int>();
            
            possibilities.Add(lastPos + new Vector2Int(1,0));
            possibilities.Add(lastPos + new Vector2Int(-1,0));
            possibilities.Add(lastPos + new Vector2Int(0,1));
            possibilities.Add(lastPos + new Vector2Int(0,-1));

            if(i != 1) possibilities.Remove(indexs[indexs.Count - 2]);

            for (int j = possibilities.Count - 1; j >= 0; j--)
            {
                if (possibilities[j].x < 0 || possibilities[j].x > 10 || possibilities[j].y < 0 ||
                    possibilities[j].y > 10)
                    possibilities.RemoveAt(j);
            }

            newPos = possibilities[Random.Range(0, possibilities.Count)];
            waypoints.Add(waypointGrid[newPos.x,newPos.y]);
            indexs.Add(newPos);
            i++;
        }

        
        return new WaypointInfo(waypoints,indexs);
    }
}
