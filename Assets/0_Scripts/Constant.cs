using Common;
using UnityEngine;

[CreateAssetMenu]
public class Constant : SingletonScriptableObject<Constant>
{
    public static int PlacingOrder = 100;
        
    public Vector2Int MapSize;
    public Vector2 TileSize;
    public float ActorMoveSpeed;
    public int MaxHP;
    public int MaxGold;
    public int FoodCost;
}
