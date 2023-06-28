using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "GAD2005/Building", order = 1)]
public class BuildingModel : ScriptableObject
{
    public BuildingType type;
    
    public string name;
    public GameObject prefab;

    public int goldCost;
    public int gemCost;
    
    public int goldGen;
    public int gemGen;

    public int tilling;
}