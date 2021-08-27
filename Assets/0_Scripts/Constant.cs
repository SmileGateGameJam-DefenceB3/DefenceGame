using UnityEngine;

[CreateAssetMenu]
public class Constant : SingletonScriptableObject<Constant>
{
    public Vector2Int MapSize;
    public Vector2 TileSize;
    public float ActorMoveSpeed;
}
