using System;
using System.Collections.Generic;
using UnityEngine;

public struct GeneralData
{
    public int currency;
    public int level;
    public int gridLevel;
    public string cityName;
    public string socialId;
    public List<string> friends;
    
    public GeneralData(int currency, int level, int gridLevel, string cityName, string socialId, List<string> friends)
    {
        this.currency = currency;
        this.level = level;
        this.gridLevel = gridLevel;
        this.cityName = cityName;
        this.friends = friends;
        this.socialId = socialId;
    }
}
public struct BuildingData
{
    public string buildingId;
    public string type;
    public Vector2Int coord;
    public int contractIndex;
    public string endTime;
    public int state;
    
    public BuildingData(string buildingId, string type, Vector2Int coord, int contractIndex, DateTime endTime, int state)
    {
        this.buildingId = buildingId;
        this.type = type;
        this.coord = coord;
        this.contractIndex = contractIndex;
        this.endTime = endTime.ToString();
        this.state = state;
    }
}