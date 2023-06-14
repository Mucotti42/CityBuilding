using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    public event Action<BuildingModel> OnStartDragging;
    public void ShapeDragging(BuildingModel building)
    {
        OnStartDragging?.Invoke(building);
    }
}