using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class TrafficSystem : MonoBehaviour
{
    public static TrafficSystem instance;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    public Vector2Int GetWaypoint(Vector2Int currentCoord, Vector2Int? previousCoord = null)
    {
        List<Vector2Int> possibilities = new List<Vector2Int>();

        possibilities = RoadManager.instance.GetRoadNeighbours(currentCoord);

        if(previousCoord.HasValue)
            possibilities.Remove(previousCoord.Value);

        if (possibilities.Count > 0)
            return possibilities[Random.Range(0, possibilities.Count)];
        
        return new Vector2Int(-1,-1);
    }
}
