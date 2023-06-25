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
        tileCoord = new Vector2Int(
            transform.GetSiblingIndex() / 10,
            transform.GetSiblingIndex() % 10);
    }

    public void Activeness(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}