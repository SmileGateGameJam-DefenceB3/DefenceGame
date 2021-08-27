using Common;
using UnityEngine;

[CreateAssetMenu]
public class Constant : SingletonScriptableObject<Constant>
{
    public static int ActorSortingOrder = 10;
    public static int PlacingActorSortingOrder = 20;
        
    public Vector2Int MapSize;
    public Vector2 TileSize;
    public float ActorMoveSpeed;
}
