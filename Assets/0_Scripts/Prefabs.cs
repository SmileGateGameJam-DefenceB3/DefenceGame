using UnityEngine;

[CreateAssetMenu]
public class Prefabs : SingletonScriptableObject<Prefabs>
{
    public TileMap TileMap;
    public Tile Tile;
    public Actor Actor;
}
