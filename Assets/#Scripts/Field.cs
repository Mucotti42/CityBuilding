using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Vector2Int coord => transform.parent.GetComponent<TileView>().tileCoord;
    public int index;
    public bool isEmpty = true;
}
