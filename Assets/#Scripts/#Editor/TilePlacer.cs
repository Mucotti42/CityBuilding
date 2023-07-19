using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TilePlacer : MonoBehaviour
{
    public GameObject Tile;
    
    public Vector2 columnOffset;
    public Vector2 rowOffset;
    public Vector2 statingPos;
    public Vector2 columRowCount;
    
    public bool place, delete = false;
    

    void Update()
    {
        if (place)
        {
            place = false;
            Place();
        }
        if (delete)
        {
            delete = false;
            Delete();
        }
    }

    private void Place()
    {
        if(transform.childCount == columRowCount.x*columRowCount.y)
        Delete();

        int index = 0;
        for (int i = 0; i < columRowCount.y; i++)
        {
            for (int j = 0; j < columRowCount.x; j++)
            {
                var pos = statingPos + (i * columnOffset);
                pos += j * rowOffset;
                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(Tile);
                tile.transform.parent = transform;
                tile.transform.localPosition = pos;
                index++;
            }
        }
        Debug.LogWarning(index);
    }

    private void Delete()
    {
        for (int i = 0; i < columRowCount.x*columRowCount.y; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
