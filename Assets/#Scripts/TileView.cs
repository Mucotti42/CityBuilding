using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    public Vector2Int tileCoord;
    private Image renderer;
    private void Awake()
    {
        renderer = GetComponent<Image>();
        CalculateCoordinates();
    }

    private void CalculateCoordinates()
    {
        
    }

    public void Activeness(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}