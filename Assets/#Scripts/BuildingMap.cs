using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMap : MonoBehaviour
{
    private int goldGen;
    private int gemGen;
    private BuildingType type;
    
    private Vector2Int coord;
    private int index;

    public float progress;
    private bool frameActiveness = false;
    private Transform parent;
    
    [SerializeField] private GameObject background, buttons, topLayer,black;
    public MapData GetSaveData()
    {
        //TODO ADD INDEX
        return new MapData((int)type, coord);
    }
    
    private void Start()
    {
        parent = transform.parent;
        StartCoroutine(IEGeneration());
    }

    public void Initialize(Vector2Int pos, int index, BuildingType type)
    {
        this.coord = pos;
        this.index = index;
        
        var model = Resources.Load<BuildingModel>("Buildings/"+type.ToString());

        goldGen = model.goldGen;
        gemGen = model.gemGen;
        
        Debug.Log("Gen started");
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
        
    }

    [ContextMenu("Activeness")]
    public void ChangeFrameActiveness()
    {
        frameActiveness = !frameActiveness;
        if (frameActiveness)
        {
            transform.parent = topLayer.transform;
            black.SetActive(true);
            background.SetActive(true);
            buttons.SetActive(true);
        }
        else
        {
            transform.parent = parent;
            black.SetActive(false);
            background.SetActive(false);
            buttons.SetActive(false);
        }
    } 
}
