using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionBar : MonoBehaviour
{
    [SerializeField] private Vector2 startingPos;
    [SerializeField] private Vector2 endPos;

    [SerializeField] private float fillAmount;
    [SerializeField] private Transform fillImage;
    private BuildingMap building;

    private void Awake()
    {
        building = transform.parent.parent.GetComponent<BuildingMap>();
    }

    void Update()
    {
        fillImage.localPosition = Vector2.Lerp(startingPos, endPos, building.progress % 1.0f);
    }
}
