using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMap : MonoBehaviour
{
    private int goldGen;
    private int gemGen;
    private BuildingType type;
    
    private Vector2Int coord;
    private int index;
    
    public MapData GetSaveData()
    {
        //TODO ADD INDEX
        return new MapData((int)type, coord);
    }

    public void Initialize(Vector2Int pos, int index, BuildingType type)
    {
        this.coord = pos;
        this.index = index;
        
        var model = Resources.Load<BuildingModel>("Buildings/"+type.ToString());

        goldGen = model.goldGen;
        gemGen = model.gemGen;
        
        Debug.Log("Gen started");
    }
}
