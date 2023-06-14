using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "GAD2005/Building", order = 1)]
public class BuildingModel : ScriptableObject
{
    public string name;
    public Sprite image;

    public int goldCost;
    public int gemCost;
    
    public int goldGen;
    public int gemGen;
    public int lifeTimeGen;

    public bool[,] tilling;
}