using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMap : MonoBehaviour
{
    private int goldGen;
    private int gemGen;
    private BuildingType type;
    private BuildingModel model;
    
    private Vector2Int coord;
    private int index;

    public float progress;
    private bool frameActiveness = false;
    private Transform parent;
    
    [SerializeField] private GameObject background, buttons;
    private GameObject topLayer, black;
    private SpriteRenderer building,constructre;

    private bool isActive;
    private Vector2Int tilling;
    // public MapData GetSaveData()
    // {
    //     return null; //new MapData(type.ToString(), index, coord,0);
    // }

    private void Awake()
    {
        building = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void Initialize(Vector2Int pos, BuildingType type, int layerIndex)
    {
        building.sortingOrder = layerIndex + 100;
        
        this.type = type;
        this.coord = pos;
        
        model = Resources.Load<BuildingModel>("Buildings/"+type.ToString());

        goldGen = model.goldGen;
        gemGen = model.gemGen;

        tilling = model.tilling;
    }

    [ContextMenu("CalculateRoad")]
    public bool CalculateRoad()
    {
        var startingCoord = coord + new Vector2Int(1, -1);
        var columnRow = MapController.instance.columnRow;
        for (int i = 0; i < tilling.y +2; i++)
        {
            for (int j = 0; j < tilling.x +2; j++)
            {
                Vector2Int currentCoord = startingCoord + new Vector2Int(-j, i);
                //if(currentCoord is {x: > 0 and < 54, y: > 0 and < 62 })
                  if(currentCoord.x > 0 && currentCoord.x < columnRow.x &&
                     currentCoord.y > 0 && currentCoord.y < columnRow.y )  
                    if (MapController.instance.GetTileContent(currentCoord.x, currentCoord.y) == TileContent.Road)
                    {
                        Debug.LogWarning("There is a road on frame");
                        return true;
                    }
            }
        }
        Debug.LogWarning("There is NO road on frame");
        return false;
    }

    public void Activate()
    {
        
    }
    
    private IEnumerator IEGeneration()
    {
        progress = 0;
        for (int i = 0; i < 1000; i++)
        {
            progress += 0.001f;
            yield return null;
        }
        MenuView.instance.gold += goldGen;
        MenuView.instance.gem += gemGen;
    }

    public void DestroyBuilding()
    {
        MapController.instance.Remove(model.tilling, coord);
        ChangeFrameActiveness();
        StopAllCoroutines();
        Destroy(gameObject);
    }

    [ContextMenu("Activeness")]
    public void ChangeFrameActiveness()
    {
        frameActiveness = !frameActiveness;
        if (frameActiveness)
        {
            transform.SetParent(topLayer.transform);
            black.SetActive(true);
            background.SetActive(true);
            buttons.SetActive(true);
        }
        else
        {
            transform.SetParent(parent);
            black.SetActive(false);
            background.SetActive(false);
            buttons.SetActive(false);
        }
    } 
}
