using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TileActiveness
{
    public bool[] fieldsActiveness;
}
public struct TilePositions
{
    public Transform[] fieldPositions;
}
public class MapController : MonoBehaviour
{
    public static MapController instance;
    
    private TileActiveness[,] tileStatus = new TileActiveness[10,10];
    private TilePositions[,] tilePositions = new TilePositions[10,10];

    private Transform _transform;
    
    private Field[] fields_1;
    private Field[] fields_2;
    private Field[] fields_4;
    private Image lastField;
    private void Awake()
    {
        if (!instance) instance = this;
        
        _transform = transform;
        InitializeGrid();
    }

    private void InitializeFields()
    {
        
    }
    private void Start()
    {
        Load();
    }

    private void InitializeGrid()
    {
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tilePositions[i, j].fieldPositions = new Transform[4]; 
                for (int k = 0; k < 4; k++)
                {
                    tilePositions[i, j].fieldPositions[k] = _transform.GetChild(index).GetChild(k).transform;
                }
                index++;
            }
        }
    }
    
    public bool Preview(Vector3 pos, int type, Transform prevObject)
    {
        Field field = FindNearestTile(pos, type);
        if (!field) return false;
        if(!CheckNeighbors(type, field))return false;

        prevObject.position = field.transform.position;
        if (lastField) lastField.enabled = false;
        lastField = field.GetComponent<Image>();
        //lastField.enabled = true;
        
        return true;
    }

    private bool CheckNeighbors(int type, Field field)
    {
        List<Field> neighbors = new List<Field>();
        int index = field.index;
        neighbors.Add(field);
        if (type == 1)
        {
        }
        else if (type == 2)
        {
            neighbors.Add(tilePositions[field.coord.x,field.coord.y].fieldPositions[index+1].GetComponent<Field>());
            
        }
        else if (type == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                neighbors.Add(tilePositions[field.coord.x,field.coord.y].fieldPositions[index+1].GetComponent<Field>());
            }
        }

        for (int i = 0; i < neighbors.Count; i++)
        {
            if (!neighbors[i].isEmpty)
                return false;
        }
        return true;
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

    private Field FindNearestTile(Vector3 pos, int type)
    {
        Transform tMin = null;
        float minDist = 95;
        List<Transform> fieldTransforms = new List<Transform>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //1
                if (type == 1)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        fieldTransforms.Add(tilePositions[i, j].fieldPositions[k]);
                    }
                }
                
                //2
                else if (type == 2)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        fieldTransforms.Add(tilePositions[i, j].fieldPositions[k]);
                        k++;
                    }
                }
                
                //4
                fieldTransforms.Add(tilePositions[i, j].fieldPositions[2]);
            }
        }

        return GetClosestTile(pos, fieldTransforms);
    }
    private Field GetClosestTile(Vector3 pos, List<Transform> fieldTransforms)
    {
        Transform closest = null;
        float minDist = 95;
        foreach (Transform field in fieldTransforms)
        {
            float dist = Vector3.Distance(field.position, pos);
            if (dist < minDist)
            {
                closest = field.transform;
                minDist = dist;
            }
        }

        if (closest)
            return closest.GetComponent<Field>();
        
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