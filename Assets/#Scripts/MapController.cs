using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct TileTransforms
{
    public Transform[] fieldTransforms;
}
public class MapController : MonoBehaviour
{
    public static MapController instance;

    [SerializeField] private Transform topLayer;
    
    private TileTransforms[,] tileTransforms = new TileTransforms[10,10];
    private Transform _transform;
    
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
        int index = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tileTransforms[i, j].fieldTransforms = new Transform[4]; 
                for (int k = 0; k < 4; k++)
                {
                    tileTransforms[i, j].fieldTransforms[k] = _transform.GetChild(index).GetChild(k).transform;
                }
                index++;
            }
        }
    }
    
    public bool Preview(Vector3 pos, int type, Transform prevObject = null)
    {
        //prevObject.position = pos;
        if (prevObject)
            prevObject.SetParent(topLayer);
        Field field = FindNearestTile(pos, type);
        if (!field) return false;
        if(!CheckNeighbors(type, field))return false;

        if (prevObject)
        {
            prevObject.SetParent(field.transform);
            prevObject.localPosition = Vector3.zero;
        }
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
            neighbors.Add(tileTransforms[field.coord.x,field.coord.y].fieldTransforms[index-1].GetComponent<Field>());
            
        }
        else if (type == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                neighbors.Add(tileTransforms[field.coord.x,field.coord.y].fieldTransforms[index+1].GetComponent<Field>());
            }
        }

        for (int i = 0; i < neighbors.Count; i++)
        {
            if (!neighbors[i].isEmpty)
                return false;
        }
        return true;
    }

    private void FillNeighbors(int type, Field field, bool isEmpty = false)
    {
        List<Field> neighbors = new List<Field>();
        int index = field.index;
        neighbors.Add(field);
        if (type == 1)
        {
        }
        else if (type == 2)
        {
            neighbors.Add(tileTransforms[field.coord.x,field.coord.y].fieldTransforms[index-1].GetComponent<Field>());
            Debug.Log("tile",tileTransforms[field.coord.x,field.coord.y].fieldTransforms[index-1].gameObject);
            
        }
        else if (type == 4)
        {
            for (int i = 0; i < 4; i++)
            {
                neighbors.Add(tileTransforms[field.coord.x,field.coord.y].fieldTransforms[index+1].GetComponent<Field>());
            }
        }

        for (int i = 0; i < neighbors.Count; i++)
        {
            neighbors[i].isEmpty = isEmpty;
        }
    }

    public void Fill(Vector3 pos, BuildingModel model)
    {
        Field field = FindNearestTile(pos, model.tilling);
        
        var building = Instantiate(model.prefab).transform;
        
        building.SetParent(topLayer);
        building.localScale = Vector3.one;
        
        building.SetParent(field.transform);
        building.localPosition = Vector3.zero;
        
        building.GetComponent<BuildingMap>().Initialize(field.coord,field.index,model.type);

        FillNeighbors(model.tilling, field);
    }

    public void Remove(int type, Vector2Int coord, int index)
    {
        Field field = tileTransforms[coord.x,coord.y].fieldTransforms[index].GetComponent<Field>();
        FillNeighbors(type,field,true);
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
                        fieldTransforms.Add(tileTransforms[i, j].fieldTransforms[k]);
                    }
                }
                
                //2
                else if (type == 2)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        fieldTransforms.Add(tileTransforms[i, j].fieldTransforms[k]);
                        k++;
                    }
                }
                
                //4
                //fieldTransforms.Add(tileTransforms[i, j].fieldTransforms[2]);
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
            //TODO EDIT
            //Fill(data.coord,(BuildingType)data.type);
        }
    }
}