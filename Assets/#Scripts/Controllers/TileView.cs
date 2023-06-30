using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    public Vector2Int tileCoord;
    private void Awake()
    {
        CalculateCoordinates();
    }

    private void CalculateCoordinates()
    {
        tileCoord = new Vector2Int(
            transform.GetSiblingIndex() / 10,
            transform.GetSiblingIndex() % 10);
    }
}