using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointPlacer : MonoBehaviour
{
    public GameObject waypoint;
    
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
        if(transform.childCount == 121)
            Delete();
        
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                var pos = statingPos + (i * columnOffset);
                pos += j * rowOffset;
                //GameObject point = (GameObject)PrefabUtility.InstantiatePrefab(waypoint);
                //point.name = "Waypoint " + i.ToString() + j.ToString();
                //point.transform.parent = transform;
                //point.transform.localPosition = pos;
            }
            
        }
    }

    private void Delete()
    {
        for (int i = 0; i < 121; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
