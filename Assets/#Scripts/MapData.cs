using UnityEngine;

public struct MapData
{
    public int type;
    public Vector2Int coord;

    public MapData(int type, Vector2Int coord)
    {
        this.type = type;
        this.coord = coord;
    }
}