using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileMap _tileMap;

    public Vector2Int Coord { get; private set; }

    public void Initialize(Vector2Int coord, TileMap tileMap)
    {
        Coord = coord;
        _tileMap = tileMap;
    }

    public Tile GetAdjacent(Vector2Int diff) => GetAdjacent(diff.x, diff.y);
    public Tile GetAdjacent(int dx, int dy) => _tileMap[Coord.x + dx, Coord.y + dy];
}
