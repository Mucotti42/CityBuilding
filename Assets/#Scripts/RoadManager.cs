using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [Serializable] public struct Road
    {
        public RoadTypes type;
        public GameObject road;
    }
    public enum RoadTypes
    {
        Horizontal,
        Vertical,
        Center,
        CTopLeft,
        CTopRight,
        CBottomLeft,
        CBottomRight,
        TExceptLeft,
        TExceptRight,
        TExceptTop,
        TExceptBottom
        
    }
    public static RoadManager instance;
    [SerializeField] private Vector2Int columnRown;

    public List<Road> roadInputs;
    private Dictionary<RoadTypes, GameObject> roadDictionary = new Dictionary<RoadTypes, GameObject>();

    public TileTest[,] roadArray;
    public bool[,] roadActiveness;
    private Transform _transform;
    private List<Vector2> activeRoads = new List<Vector2>();
    private List<Vector2Int>[,] roadNeighbours;

    private Vector2Int lastMainCoord;
    private void Awake()
    {
        if (!instance) instance = this;
        _transform = transform;
    }

    private void Start()
    {
        SetRoadDictionary();
        SetRoadArray();
    }

    private void SetRoadDictionary()
    {
        for (int i = 0; i < roadInputs.Count; i++)
        {
            roadDictionary.Add(roadInputs[i].type, roadInputs[i].road);
        }
    }

    private void SetRoadArray()
    {
        roadArray = new TileTest[columnRown.x, columnRown.y];
        roadActiveness = new bool[columnRown.x, columnRown.y];
        roadNeighbours = new List<Vector2Int>[columnRown.x, columnRown.y];
        var index = 0;
        for (int i = 0; i < columnRown.y; i++)
        {
            for (int j = 0; j < columnRown.x; j++)
            {
                roadArray[j, i] = _transform.GetChild(index).GetComponent<TileTest>();
                Debug.LogWarning(j +" "+ i + " " + index);
                index++;
            }
        }
    }

    public RoadTypes RoadCreation(Vector2Int coord, bool isNeighbour = false)
    {
        Debug.LogWarning(coord);
        RoadTypes type = RoadTypes.Horizontal;
        int neighbourCount = 0;
        List<bool> neighbourActiveness = new List<bool>();
        
        bool left = CheckGrid(new Vector2Int(coord.x + 1, coord.y),TileContent.Road);
        neighbourActiveness.Add(left);

        if (roadNeighbours[coord.x, coord.y] == null)
            roadNeighbours[coord.x, coord.y] = new List<Vector2Int>();
        
        if (left)
        {
            neighbourCount++;
            if(!isNeighbour)
                roadNeighbours[coord.x,coord.y].Add(new Vector2Int(coord.x + 1, coord.y));
        }
        
        bool right = CheckGrid(new Vector2Int(coord.x - 1, coord.y),TileContent.Road);
        neighbourActiveness.Add(right);
        if (right)
        {
            neighbourCount++;
            if(!isNeighbour)
                roadNeighbours[coord.x,coord.y].Add(new Vector2Int(coord.x - 1, coord.y));
        }
        
        bool top = CheckGrid(new Vector2Int(coord.x, coord.y - 1),TileContent.Road);
        neighbourActiveness.Add(top);
        if (top)
        {
            neighbourCount++;
            if(!isNeighbour)
                roadNeighbours[coord.x,coord.y].Add(new Vector2Int(coord.x, coord.y - 1));
        }
        
        bool down = CheckGrid(new Vector2Int(coord.x, coord.y + 1),TileContent.Road);
        neighbourActiveness.Add(down);
        if (down)
        {
            neighbourCount++;
            if(!isNeighbour)
                roadNeighbours[coord.x,coord.y].Add(new Vector2Int(coord.x, coord.y + 1));
        }

        switch (neighbourCount)
        {
            case 0:
            {
                type = RoadTypes.Horizontal;
                break;
            }
            case 1:
            {
                if (left || right)
                    type = RoadTypes.Horizontal;
                else
                    type = RoadTypes.Vertical;
                break;
            }
            case 2:
            {
                if (left && right)
                    type = RoadTypes.Horizontal;
                else if (top && down)
                    type = RoadTypes.Vertical;
                else if (top)
                {
                    if (left)
                        type = RoadTypes.CTopLeft;
                    
                    else if (right)
                        type = RoadTypes.CTopRight;
                }
                else if (down)
                {
                    if (left)
                        type = RoadTypes.CBottomLeft;
                    
                    else if (right)
                        type = RoadTypes.CBottomRight;
                }
                break;
            }
            case 3:
            {
                if (!left)
                    type = RoadTypes.TExceptLeft;
                if (!right)
                    type = RoadTypes.TExceptRight;
                if (!top)
                    type = RoadTypes.TExceptTop;
                if (!down)
                    type = RoadTypes.TExceptBottom;
                break;
            }
            case 4:
            {
                type = RoadTypes.Center;
                break;
            }
        }

        SetGrid(coord.x, coord.y);

        if (!isNeighbour)
        {
            lastMainCoord = coord;
            if (left)  RoadCreation(new Vector2Int(coord.x + 1, coord.y),true);
            if (right) RoadCreation(new Vector2Int(coord.x - 1, coord.y),true);
            if (top)   RoadCreation(new Vector2Int(coord.x, coord.y -1),true);
            if (down)  RoadCreation(new Vector2Int(coord.x, coord.y +1),true);
        }
        
        if (isNeighbour)
        {
            roadNeighbours[coord.x,coord.y].Add(new Vector2Int(lastMainCoord.x,lastMainCoord.y));
            DestroyRoad(coord,false);
        }
        SpawnRoad(coord,type);
        return type;
    }

    private bool CheckGrid(Vector2Int coord, TileContent content)
    {
        if (coord is { x: > 0 and < 54, y: > 0 and < 62 })
        return MapController.instance.GetTileContent(coord.x, coord.y) == content;
            
        return false;
    }

    private void SetGrid(int x, int y)
    {
        MapController.instance.SetTileContent(x, y, TileContent.Road);
    }
    public void SpawnRoad(Vector2Int coord,RoadTypes type)
    {
        roadArray[coord.x,coord.y].EditRoadPosition(Instantiate(roadDictionary[type]).transform);
    }

    public void DestroyRoad(Vector2Int coord, bool destroyOnArray = true)
    {
        roadArray[coord.x,coord.y].DestroyRoad();
        if(destroyOnArray)
        RoadDestruction(coord);
    }
    public void RoadDestruction(Vector2Int coord)
    {
        roadActiveness[coord.x, coord.y] = false;
    }

    public List<Vector2Int> GetRoadNeighbours(Vector2Int coord)
    {
        return roadNeighbours[coord.x, coord.y];
    }

}