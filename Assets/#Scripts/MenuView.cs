using System;
using Unity.VisualScripting;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    public static MenuView instance;
    
    private int _gold;
    private int _gem;

    public int gold 
    { 
        get => _gold; 
        set { _gold = value; GemGoldFeedBack( true,_gold - value);} 
    }
    public int gem 
    { 
        get => _gem; 
        set { _gem = value; GemGoldFeedBack( false,_gem - value);} 
    }

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        GetCurrencyData();
    }

    public void Restart()
    {
        //TODO: Button listener   
    }

    private void GetCurrencyData()
    {
        Vector2Int currency = Utils.instance.LoadCurrencyData();
        _gold = currency.x;
        _gem = currency.y;
        EventManager.instance.UpdateCurrency();
    }

    private void GemGoldFeedBack(bool gold, int value)
    {
        EventManager.instance.UpdateCurrency();
    }
}