using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TileMap _tileMap;

    public Vector2Int Coord { get; private set; }

    public void Initialize(Vector2Int coord, TileMap tileMap)
    {
        Coord = coord;
        _tileMap = tileMap;

        var tileSize = Constant.Instance.TileSize;
        transform.localScale = tileSize;
        transform.localPosition = new Vector3(tileSize.x * coord.x, tileSize.y * coord.y, 1f);
    }

    public Tile GetAdjacent(Vector2Int diff) => GetAdjacent(diff.x, diff.y);
    public Tile GetAdjacent(int dx, int dy) => _tileMap[Coord.x + dx, Coord.y + dy];
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        InputManager.Instance.ReportEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.Instance.ReportExit(this);
    }
}
