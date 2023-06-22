using UnityEngine;
using UnityEngine.UI;

public struct TileActiveness
{
    public bool[] fieldsActiveness;
}
public struct TilePositions
{
    public Vector2[] fieldPositions;
}
public class MapController : MonoBehaviour
{
    public static MapController instance;
    
    private TileActiveness[,] tileStatus = new TileActiveness[10,10];
    private TilePositions[,] tilePositions = new TilePositions[10,10];

    private Transform _transform;
    private void Awake()
    {
        if (!instance) instance = this;
        
        _transform = transform;
        InitializeGrid();
        Load();
    }

    private void InitializeGrid()
    {
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tilePositions[i, j].fieldPositions = new Vector2[4]; 
                for (int k = 0; k < 4; k++)
                {
                    tilePositions[i, j].fieldPositions[k] = _transform.GetChild(index).GetChild(k).localPosition;
                }
                index++;
            }
        }
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
        var buildingData = Utils.instance.LoadMapData();
        for (int i = 0; i < buildingData.Count; i++)
        {
            var data = buildingData[i];
            Fill(data.coord,(BuildingType)data.type);
        }
    }
}