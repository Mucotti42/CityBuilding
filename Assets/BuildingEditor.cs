using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingEditor : MonoBehaviour
{
    public List<Transform> tiles = new List<Transform>();

    private void Start()
    {
        //EditLayers();
    }

    private void EditLayers()
    {
        for (int i = 0; i < 100; i++)
        {
            tiles.Add(transform.GetChild(i));
        }

        for (int i = 99; i > -1; i--)
        {
            var tile = tiles[99-i];
            tile.SetSiblingIndex(i);
            List<Transform> childeren = new List<Transform>();
            for (int j = 0; j < 4; j++)
            {
                childeren.Add(tile.GetChild(j));
            }

            for (int j = 3; j > -1; j--)
            {
                childeren[3-j].SetSiblingIndex(j);
            }
        }
    }
}
