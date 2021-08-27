using System;
using NaughtyAttributes;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public Vector2Int Size { get; private set; }
    private Tile[,] _tiles;

    public int Width => Size.x;
    public int Height => Size.y;

    public Tile this[int x, int y]
    {
        get
        {
            if (!IsInBound(x, y))
            {
                return null;
            }

            return _tiles[x, y];
        }
    }

    public bool IsInBound(int x, int y) => 0 <= x && x < Width && 0 <= y && y < Height;

    private void Awake()
    {
        Initialize(true);
    }

    public void Initialize(bool nonDestroy)
    {
        if (nonDestroy)
        {
            transform.DestroyAllChildren();
        }

        Size = Constant.Instance.MapSize;
        _tiles = new Tile[Size.x, Size.y];

        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                var tile = Instantiate(Prefabs.Instance.Tile, transform);
                tile.Initialize(new Vector2Int(x, y), this);
                tile.gameObject.name = $"Tile ({x},{y})";
                tile.transform.localPosition = new Vector3(Constant.Instance.TileSize.x * x, Constant.Instance.TileSize.y * y, 1f);
                _tiles[x, y] = tile;
            }
        }
    }

    [Button]
    private void DummySpawn()
    {
        var actor = Instantiate(Prefabs.Instance.Actor);
        actor.Initialize(_tiles[0, 2], 1);
        actor.StartMoveToNextTile();
    }
}
