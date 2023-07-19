using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TileTransforms
{
    public Transform[] fieldTransforms;
}

public enum TileContent
{
    Empty,
    Road,
    Building
}
public class MapController : MonoBehaviour
{
    public static MapController instance;
    
    private Transform[,] tileTransforms;
    private Transform _transform;
    public TileContent[,] tileContents;
    [SerializeField] private Vector2Int columnRown;
    
    private void Awake()
    {
        if (!instance) instance = this;
        
        _transform = transform;
        InitializeGrid();
    }

    private void Start()
    {
        Load();
    }

    private void InitializeGrid()
    {
        tileContents = new TileContent[columnRown.x, columnRown.y];
        tileTransforms = new Transform[columnRown.x, columnRown.y];
        
        int index = 0;
        for (int i = 0; i < columnRown.y; i++)
        {
            for (int j = 0; j < columnRown.x; j++)
            {
                tileTransforms[j, i] = _transform.GetChild(index);
                index++;
            }
        }
    }
    
    public bool Preview(Vector3 pos, Vector2Int tilling, Transform prevObject = null, SpriteRenderer renderer = null)
    {
        if (renderer)
            renderer.sortingOrder = 4000;
        TileTest tile = GetClosestTile(pos);
        if (!tile) return false;
        if(!CheckNeighbors(tilling, tile.coord))return false;

        if (prevObject)
            prevObject.position = tile.transform.position;

        if (renderer)
            renderer.sortingOrder = tile.transform.GetSiblingIndex();
        
        return true;
    }

    private bool CheckNeighbors(Vector2Int tilling, Vector2Int coord)
    {
        for (int j = 0; j < tilling.y; j++)
        {
            for (int i = 0; i < tilling.x; i++)
            {
                if (coord.x - i < 0 || coord.x - i > columnRown.x || coord.y + j < 0 || coord.y + j >= columnRown.y)
                    return false;
                if (tileContents[coord.x - i, coord.y + j] != TileContent.Empty)
                {
                    Debug.Log(coord + " " + tileContents[coord.x - i, coord.y + j]);
                    return false;
                }
            }
        }
        return true;
    }

    private void FillNeighbors(Vector2Int tilling, Vector2Int coord,TileContent content)
    {
        for (int j = 0; j < tilling.y; j++)
        {
            for (int i = 0; i < tilling.x; i++)
            {
                SetTileContent(coord.x - i, coord.y + j,content);
            }
        }
    }

    public void Fill(Vector3 pos, BuildingModel model)
    {
        TileTest tile = GetClosestTile(pos);
        MenuView.instance.gold -= model.goldCost;
        MenuView.instance.gem -= model.gemCost;
        Fill(tile, model, model.consTime);
    }

    public void Fill(TileTest tile, BuildingModel model,int remainingConsTime)
    {
        var building = Instantiate(model.prefab).transform;
        building.transform.position = tile.transform.position;
        int index = GetTile(tile.coord + new Vector2Int(0,model.tilling.y)).GetSiblingIndex();
        //building.SetParent(topLayer);
        //building.localScale = Vector3.one;
        
        //building.SetParent(field.transform);
        //building.localPosition = Vector3.zero;
        
        building.GetComponent<BuildingMap>().Initialize(tile.coord,model.type, index);

        FillNeighbors(model.tilling, tile.coord,TileContent.Building);
    }

    public void Remove(Vector2Int tilling, Vector2Int coord)
    {
        //Field field = tileTransforms[coord.x,coord.y].fieldTransforms[index].GetComponent<Field>();
        //FillNeighbors(type,field,true);
        FillNeighbors(tilling, coord, TileContent.Empty);
    }
    
    private TileTest GetClosestTile(Vector3 pos)
    {
        Transform closest = null;
        float minDist = 5;
        foreach (Transform tile in tileTransforms)
        {
            float dist = Vector3.Distance(tile.position, pos);
            if (dist < minDist)
            {
                closest = tile.transform;
                minDist = dist;
            }
        }

        if (closest)
            return closest.GetComponent<TileTest>();
        
        return null;
    }

    private void Load()
    {
        // var buildingData = Utils.instance.LoadMapData();
        // for (int i = 0; i < buildingData.Count; i++)
        // {
        //     var data = buildingData[i];
        //     var model = Resources.Load<BuildingModel>("Buildings/"+data.type);
        //     var remainingConsTime = 0;
        //     Fill(field,model,remainingConsTime);
        // }
    }

    public Transform GetTile(Vector2Int coord)
    {
        return tileTransforms[coord.x, coord.y];
    }

    public TileContent GetTileContent(int x, int y)
    {
        return tileContents[x, y];
    }
    public void SetTileContent(int x, int y, TileContent content)
    {
        tileContents[x, y] = content;
    }
}