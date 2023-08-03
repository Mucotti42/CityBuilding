using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    public GameObject Tile;
    
    public Vector2 columnOffset;
    public Vector2 rowOffset;
    public Vector2 statingPos;
    public int columRowCount;
    
    public float rate;

    private void Awake()
    {
        Place(columRowCount);
    }

    private void Place(int columnCount)
    {
        float localScale = 6.433344f;
        if (columnCount != 10)
        {
            rate = 10f / columnCount;
            var firstRow = rowOffset;
        
            rowOffset *= rate;
            columnOffset *= rate;
            statingPos -= firstRow - rowOffset;
            localScale *= rate;
        }

        int index = 0;
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                var pos = statingPos + (i * columnOffset);
                pos += j * rowOffset;
                var pos2 = new Vector3(pos.x, pos.y, -1);
                GameObject tile = Instantiate(Tile);
                tile.transform.parent = transform;
                tile.transform.localPosition = pos2;
                tile.transform.localScale = Vector3.one * localScale;
                index++;
            }
        }
        MapController.instance.columnRow = new Vector2Int(columnCount, columnCount);
        Debug.LogWarning(index);
    }

    [ContextMenu("Add")]
    public void Add()
    {
        int count = 4;
        int childCount = transform.childCount;
        int row = (int)math.sqrt(childCount);
        int toAdd = (int)(math.pow(row + count, 2) - childCount);
        float scale = transform.GetChild(0).localScale.x;
        rate = (int)math.sqrt(childCount) / (math.sqrt(childCount) + count);

        List<Transform> tiles = new List<Transform>();
        for (int i = 0; i < childCount; i++)
            tiles.Add(transform.GetChild(i));
        
        for (int j = 0; j < toAdd; j++) 
        {
            GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(Tile);
            tile.transform.parent = transform;
        }
        for (int i = tiles.Count-1; i > -1; i--)
        {
            var tile = tiles[i];
            Vector2Int coord = tile.GetComponent<TileTest>().coord + new Vector2Int(2,2);
            int x = coord.y * (int)math.sqrt(transform.childCount) + coord.x;
            tile.gameObject.name = x.ToString();
            tile.SetSiblingIndex(x);
        }
        EditPositions(scale);
    }
    
    public void EditPositions(float scale)
    {
        var firstRow = rowOffset;
        
        rowOffset *= rate;
        columnOffset *= rate;
        statingPos -= firstRow - rowOffset;

        int columnRow = (int)math.sqrt(transform.childCount);
        MapController.instance.columnRow = new Vector2Int(columnRow, columnRow);
        float localScale = transform.localScale.x;
        localScale *= rate;

        transform.localScale = localScale * Vector3.one;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TileTest>().CalculateCoord();
        }
        int index = 0;
        for (int i = 0; i < columnRow; i++)
        {
            for (int j = 0; j < columnRow; j++)
            {
                
                Transform tile = transform.GetChild(index);
                tile.localScale = Vector3.one * scale;
                var pos = statingPos + (i * columnOffset);
                pos += j * rowOffset;
                Vector3 pos2 = new Vector3(pos.x, pos.y, -1);
                tile.position = pos2;
                index++;
            }
        }

        MapController.instance.InitializeGrid(true);
    }
}
