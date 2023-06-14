using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    #region Player Pref Keys

    private const string prefGold     = "prefGold";
    private const string prefGem      = "prefGem";
    private const string prefCount    = "prefCount";
    private const string prefType     = "prefType";
    private const string prefPosX     = "prefPosX";
    private const string prefPosY     = "prefPosY";
    private const string prefLifeTime = "prefLifeTime";

    #endregion
    
    public static Utils instance;
    
    private void Awake()
    {
        if (!instance) instance = this;
    }
    
    private void SaveData()
    {
        
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public Vector2Int LoadCurrencyData()
    {
        return new Vector2Int();
    }

    public List<Dictionary<int, MapData>> LoadMapData()
    {
        return new List<Dictionary<int, MapData>>();
    }

    private void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }

}