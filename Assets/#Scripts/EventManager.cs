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
    public void StartDragging(BuildingModel building)
    {
        Debug.LogWarning("event");
        OnStartDragging?.Invoke(building);
    }
    
    public event Action OnCurrencyUpdated;
    public void UpdateCurrency()
    {
        OnCurrencyUpdated?.Invoke();
    }
}