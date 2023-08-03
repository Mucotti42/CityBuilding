using System;
using System.Collections.Generic;
using UnityEngine;
public class Utils : MonoBehaviour
{
    public enum BuildingState
    {
        OnRent,
        ReadyToCollect,
        Contract,
        Construction
    }
    public enum GeneralDatas
    {
        Currency,
        Level,
        GridLevel,
        Friends,
        SocialId
    }
    #region Player Pref Keys

    private const string prefGold     = "prefGold";
    private const string prefGem      = "prefGem";
    private const string prefCount    = "prefCount";
    
    private const string prefType     = "prefType";
    private const string prefPosX     = "prefPosX";
    private const string prefPosY     = "prefPosY";
    private const string prefindex = "prefIndex";

    #endregion
    
    public static Utils instance;

    [SerializeField] private Vector2Int startingCurrency = new Vector2Int(10,10);
    public GameObject canvas;
    private void Awake()
    {
        if (!instance) instance = this;
        canvas.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            SaveData();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(prefGold,MenuView.instance.gold);
        PlayerPrefs.SetInt(prefGem,MenuView.instance.gem);
        
        #region BuildingDataSave

        List<BuildingData> mapDatas = new List<BuildingData>();
        var buildings = FindObjectsOfType<BuildingMap>();
        for (int i = 0; i < buildings.Length; i++)
        {
            //mapDatas.Add(buildings[i].GetSaveData());
        }

        PlayerPrefs.SetInt(prefCount,mapDatas.Count);
        for (int i = 0; i < mapDatas.Count; i++)
        {
            var data = mapDatas[i];
            PlayerPrefs.SetString(i.ToString() + prefType,data.type);
            PlayerPrefs.SetInt(i.ToString() + prefPosX,data.coord.x);
            PlayerPrefs.SetInt(i.ToString() + prefPosY,data.coord.y);
            //PlayerPrefs.SetInt(i.ToString() + prefindex,data.index);
        }
        PlayerPrefs.Save();
        #endregion
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public Vector2Int LoadCurrencyData()
    {
        return new Vector2Int(
            PlayerPrefs.GetInt(prefGold,startingCurrency.x),
            PlayerPrefs.GetInt(prefGem,startingCurrency.y)
            );
    }

    public List<BuildingData> LoadMapData()
    {
        var buildingCount = PlayerPrefs.GetInt(prefCount, 0);
        var loadedData = new List<BuildingData>();
        for (int i = 0; i < buildingCount; i++)
        {
            var data = new BuildingData();
            data.type = PlayerPrefs.GetString(i.ToString() + prefType);
            data.coord.x = PlayerPrefs.GetInt(i.ToString() + prefPosX,0);
            data.coord.y = PlayerPrefs.GetInt(i.ToString() + prefPosY,0);
            //data.index = PlayerPrefs.GetInt(i.ToString() + prefindex,0);
            
            loadedData.Add(data);
        }

        return loadedData;
    }
    
    

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }

}