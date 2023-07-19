using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarAI : MonoBehaviour
{
    [SerializeField] private Vector2Int startingCoord;

    private Transform _transform;
    private Vector2Int currentCoord;
    private Vector2Int previousCoord;
    private Vector2Int nextCoord;

    private Transform currentTransform;
    private Transform nextTransform;
    public Vector3 offset;

    public Vector3 up = new Vector2(3.56f,2);
    public Vector3 down = new Vector2(-3.56f,-6.3f);
    public Vector3 right = new Vector2(5.95f,-8.57f);
    public Vector3 left = new Vector2(1.7f, -7.93f);

    private int lastIndex = 0;
    public Transform testSprite;
    private void Awake()
    {
        currentCoord = startingCoord;
        _transform = transform;
    }

    private void OnEnable()
    {
        currentCoord = TrafficSystem.instance.GetWaypoint(currentCoord);
        var possibilities = RoadManager.instance.GetRoadNeighbours(currentCoord);
        if (possibilities.Count > 0)
        {
            previousCoord = possibilities[Random.Range(0, possibilities.Count)];
            StartCoroutine(IEDrive());
        }
    }

    private bool GetInfo()
    {
        nextCoord = TrafficSystem.instance.GetWaypoint(currentCoord,previousCoord);
        if(nextCoord == new Vector2Int(-1, -1))
            return false;
        
        currentTransform = MapController.instance.GetTile(currentCoord);
        nextTransform = MapController.instance.GetTile(nextCoord);
        return true;
    }
    private IEnumerator IEDrive()
    {
        while (true)
        {
            if(!GetInfo())
                break;
            CalculateRotationOffsetLayer(currentTransform, nextTransform);
            for (int j = 0; j < 1000; j++)
            {
                _transform.position = Vector3.Lerp(currentTransform.position + new Vector3(1.279f,0,0),nextTransform.position + new Vector3(1.279f,0,0),0.001f*j)+offset;
                testSprite.position = Vector3.Lerp(currentTransform.position + new Vector3(1.279f,0,0), nextTransform.position + new Vector3(1.279f,0,0), 0.001f * j);
                yield return null;
            }

            previousCoord = currentCoord;
            currentCoord = nextCoord;
            
        }
    }

    private void CalculateRotationOffsetLayer(Transform from, Transform to)
    {
        Vector3 direction = to.position - from.position;
        int index;
        if (direction.x < 0)
        {
            if (direction.y < 0)
            {
                index = 0;
                offset = left;
                CalculateLayer(from.GetSiblingIndex());
            }
            else
            {
                index = 1;
                offset = up;
                CalculateLayer(to.GetSiblingIndex());
            }
        }
        else
        {
            if (direction.y < 0)
            {
                index = 2;
                offset = down;
                CalculateLayer(from.GetSiblingIndex());
            }
            else
            {
                index = 3;
                offset = right;
                CalculateLayer(to.GetSiblingIndex());
            }
        }
        transform.GetChild(lastIndex).gameObject.SetActive(false);
        transform.GetChild(index).gameObject.SetActive(true);
        lastIndex = index;
    }
    private void CalculateLayer(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = index + 99;
        }        
    }
}
