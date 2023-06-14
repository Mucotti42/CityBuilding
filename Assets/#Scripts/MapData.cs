using UnityEngine;

public struct MapData
{
    public int type;
    public int lifetimeGen;
    public Vector2Int pos;

    public MapData(int type, int lifetimeGen, Vector2Int pos)
    {
        this.type = type;
        this.lifetimeGen = lifetimeGen;
        this.pos = pos;
    }
    
}