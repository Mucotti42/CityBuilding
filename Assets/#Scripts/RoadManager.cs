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

    public List<Road> roadInputs;
    private Dictionary<RoadTypes, GameObject> roadDictionary;
    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        SetRoadDictionary();
    }

    private void SetRoadDictionary()
    {
        roadDictionary = new Dictionary<RoadTypes, GameObject>();
        for (int i = 0; i < roadInputs.Count; i++)
        {
            roadDictionary.Add(roadInputs[i].type, roadInputs[i].road);
        }
    }

    public RoadTypes RoadCreation(Vector2Int coord, bool isNeighbour = false)
    {
        RoadTypes type = RoadTypes.Horizontal;
        int neighbourCount = 0;
        List<bool> neighbourActiveness = new List<bool>();
        
        bool left = CheckGrid(new Vector2Int(coord.x + 1, coord.y),TileContent.Road);
        neighbourActiveness.Add(left);
        
        if (left)
            neighbourCount++;
        
        bool right = CheckGrid(new Vector2Int(coord.x - 1, coord.y),TileContent.Road);
        neighbourActiveness.Add(right);
        if (right)
            neighbourCount++;
        
        bool top = CheckGrid(new Vector2Int(coord.x, coord.y - 1),TileContent.Road);
        neighbourActiveness.Add(top);
        if (top)
            neighbourCount++;
        
        bool down = CheckGrid(new Vector2Int(coord.x, coord.y + 1),TileContent.Road);
        neighbourActiveness.Add(down);
        if (down)
            neighbourCount++;

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
            if (left)  RoadCreation(new Vector2Int(coord.x + 1, coord.y),true);
            if (right) RoadCreation(new Vector2Int(coord.x - 1, coord.y),true);
            if (top)   RoadCreation(new Vector2Int(coord.x, coord.y -1),true);
            if (down)  RoadCreation(new Vector2Int(coord.x, coord.y +1),true);
        }
        
        if (isNeighbour)
        {
            DestroyRoad(coord,false);
        }
        SpawnRoad(coord,type);
        return type;
    }

    private bool CheckGrid(Vector2Int coord, TileContent content)
    {
        //if (coord is { x: > 0 and < 54, y: > 0 and < 62 })
            
        if(coord.x >= MapController.instance.columnRow.x || coord.x < 0 || coord.y >= MapController.instance.columnRow.y || coord.y < 0)
        return false;
            
        return MapController.instance.GetTileContent(coord.x, coord.y) == content;
    }

    private void SetGrid(int x, int y) 
    {
        MapController.instance.SetTileContent(x, y, TileContent.Road);
    }
    public void SpawnRoad(Vector2Int coord,RoadTypes type)
    {
        MapController.instance.GetTile(coord).GetComponent<TileTest>().EditRoadPosition(Instantiate(roadDictionary[type]).transform);
    }

    public void DestroyRoad(Vector2Int coord, bool destroyOnArray = true)
    {
        MapController.instance.GetTile(coord).GetComponent<TileTest>().DestroyRoad();
        
        if(destroyOnArray)
        MapController.instance.SetTileContent(coord.x,coord.y,TileContent.Empty);
    }

    public List<Vector2Int> GetRoadNeighbours(Vector2Int coord)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        if (CheckGrid(new Vector2Int(coord.x + 1, coord.y), TileContent.Road))
            neighbours.Add(new Vector2Int(coord.x + 1, coord.y));
        if (CheckGrid(new Vector2Int(coord.x - 1, coord.y), TileContent.Road))
            neighbours.Add(new Vector2Int(coord.x - 1, coord.y));
        if (CheckGrid(new Vector2Int(coord.x, coord.y -1), TileContent.Road))
            neighbours.Add(new Vector2Int(coord.x, coord.y -1));
        if (CheckGrid(new Vector2Int(coord.x, coord.y +1), TileContent.Road))
            neighbours.Add(new Vector2Int(coord.x, coord.y +1));
            
        return neighbours;
    }

}