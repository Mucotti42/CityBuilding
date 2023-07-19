using UnityEngine;

public struct MapData
{
    public string type;
    public int index;
    public Vector2Int coord;
    public int remainingConsTime;

    public MapData(string type,int index, Vector2Int coord, int remainingConsTime)
    {
        this.type = type;
        this.index = index;
        this.coord = coord;
        this.remainingConsTime = remainingConsTime;
    }
}