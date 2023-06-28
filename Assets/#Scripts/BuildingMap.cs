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
    public MapData GetSaveData()
    {
        return new MapData(type.ToString(), index, coord);
    }
    
    private void Start()
    {
        topLayer = GameObject.Find("TopLayer");
        black = GameObject.Find("Black").transform.GetChild(0).gameObject;
        parent = transform.parent;
        StartCoroutine(IEGeneration());
    }

    public void Initialize(Vector2Int pos, int index, BuildingType type)
    {
        this.coord = pos;
        this.index = index;
        
        model = Resources.Load<BuildingModel>("Buildings/"+type.ToString());

        goldGen = model.goldGen;
        gemGen = model.gemGen;
        
        StartCoroutine(IEGeneration());
    }

    private IEnumerator IEGeneration()
    {
        progress = 0;
        while (true)
        {
            for (int i = 0; i < 10000; i++)
            {
                progress += 0.0001f;
                yield return null;
            }

            MenuView.instance.gold += goldGen;
            MenuView.instance.gem += gemGen;
        }
    }

    public void DestroyBuilding()
    {
        MapController.instance.Remove(model.tilling, coord,index);
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
