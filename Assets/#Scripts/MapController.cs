using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    
    private bool[,] tileStatus = new bool[10,10];
    private Image[,] images = new Image[10,10];

    public Sprite emptyTile;
    public Sprite buildingTile;

    private void Awake()
    {
        if (!instance) instance = this;
    }
    
    public bool Preview(Vector2Int pos)
    {
        
        return false;
    }

    public void Fill(Vector2Int pos, BuildingType type)
    {
        
    }
    
    public void Fill(Vector2Int pos, BuildingModel building)
    {
        
    }

    public void Remove()
    {
        //TODO: Button listener
    }

    private TileView FindNearestTile()
    {
        return null;
    }

    private void Load()
    {
        
    }
    
    
}