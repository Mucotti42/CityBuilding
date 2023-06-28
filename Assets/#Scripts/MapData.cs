using UnityEngine;

public struct MapData
{
    public string type;
    public int index;
    public Vector2Int coord;

    public MapData(string type,int index, Vector2Int coord)
    {
        this.type = type;
        this.index = index;
        this.coord = coord;
    }
}