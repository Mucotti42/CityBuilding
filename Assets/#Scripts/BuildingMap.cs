using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMap : MonoBehaviour
{
    private int goldGen;
    private int gemGen;
    private BuildingType type;
    
    private Vector2Int coord;
    
    public MapData GetSaveData()
    {
        return new MapData((int)type, coord);
    }
}
