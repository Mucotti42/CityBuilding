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
        if(transform.childCount == 100)
        Delete();
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var pos = statingPos + (i * columnOffset);
                pos += j * rowOffset;
                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(Tile);
                tile.transform.parent = transform;
                tile.transform.localPosition = pos;
            }
            
        }
    }

    private void Delete()
    {
        for (int i = 0; i < 100; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
