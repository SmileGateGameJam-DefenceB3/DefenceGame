using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu]
public class TileMapGenerator : SingletonScriptableObject<TileMapGenerator>
{
    [SerializeField] private TileMap _tileMapPrefab;

    [Button]
    public TileMap Generate()
    {
        var tileMap = Instantiate(_tileMapPrefab);
        tileMap.Initialize(false);
        return tileMap;
    }
}
