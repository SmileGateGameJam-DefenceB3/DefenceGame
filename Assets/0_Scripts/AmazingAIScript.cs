using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AmazingAIScript : SingletonMonoBehaviour<AmazingAIScript>
{
    [SerializeField] private int _initialGold;
    [SerializeField] private float _minSpawnDelay = 2f;
    [SerializeField] private float _maxSpawnDelay = 3f;
    
    private readonly List<Tile> _availableTiles = new List<Tile>();
    private float _nextSpawnTime;
    private readonly Queue<Tile> _actQueue = new Queue<Tile>();

    private int _rabbitRushCount = 2;

    public int InitialGold => _initialGold;
    
    private int _gold;
    public int Gold
    {
        get => _gold;
        set
        {
            _gold = Mathf.Clamp(value, 0, InitialGold);
            OnGoldChanged?.Invoke(_gold);
        }
    }
    
    public UnityEvent<int> OnGoldChanged = new UnityEvent<int>();

    public void OnPlayerSpawned(Tile tile, Actor actor)
    {
        _actQueue.Enqueue(tile);
        _nextSpawnTime -= actor.Damage * 0.2f;
    }

    private void Start()
    {
        foreach (var tile in InGameManager.TileMap.Tiles)
        {
            if (tile.Coord.x > 1 + Constant.Instance.MapSize.x / 2)
            {
                _availableTiles.Add(tile);
            }
        }

        Gold = InitialGold;
    }

    public void Run()
    {
        StartCoroutine(nameof(RunCo));
    }

    public void Stop()
    {
        StopCoroutine(nameof(RunCo));
    }

    private IEnumerator RunCo()
    {
        _nextSpawnTime = Time.time + 5f;

        while (true)
        {
            if (Gold == 0)
            {
                yield return null;
                continue;
            }

            if (Time.time > _nextSpawnTime)
            {
                _nextSpawnTime = Time.time + Random.Range(_minSpawnDelay, _maxSpawnDelay);

                if (_rabbitRushCount > 0 && Random.Range(0, 1f) < 0.1f)
                {
                    _rabbitRushCount--;
                    int rabbitCount = Random.Range(6, 9);
                    yield return HorizontalRabbitRush(rabbitCount, GetRandomTile());
                }
                else
                {
                    var defenceTile = GetDefenceTile();
                    if (defenceTile == null || Random.Range(0, 1f) < 0.05f)
                    {
                        SpawnRandom(GetRandomTile(), GetRandomLevel(30, 30, 20, 5));
                    }
                    else
                    {
                        int level = GetRandomLevel(80, 13, 4, 3);
                        SpawnRandom(defenceTile, level);
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator HorizontalRabbitRush(int count, Tile tile)
    {
        for (int i = 0; i < count; i++)
        {
            Spawn(ActorType.Rabbit, 1, tile);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private int GetRandomLevel(int l1, int l2, int l3, int l4)
    {
        int random = Random.Range(0, l1 + l2 + l3 + l4);
        if (random < l1)
        {
            return 1;
        }

        if (random < l1 + l2)
        {
            return 2;
        }

        if (random < l1 + l2 + l3)
        {
            return 3;
        }

        return 4;
    }
    
    private Tile GetRandomTile()
    {
        return _availableTiles[Random.Range(0, _availableTiles.Count)];
    }
    
    private Tile GetDefenceTile()
    {
        var covered = new HashSet<int>();

        foreach (var actor in InGameManager.ActorManager.GetActors(Team.CPU))
        {
            covered.Add(actor.Y);
        }

        foreach (var actor in InGameManager.ActorManager.GetActors(Team.Player))
        {
            if (!covered.Contains(actor.Y))
            {
                return InGameManager.TileMap[7, actor.Y];
            }
        }

        return null;
    }

    private void SpawnRandom(Tile tile, int level = 1)
    {
        var type = Random.Range((int) ActorType.Rabbit, (int) ActorType.Elephant + 1);
        Spawn((ActorType) type, level, tile);
    }

    private void Spawn(ActorType type, int level, Tile tile)
    {
        var actorData = InGameManager.ActorManager.GetActorData(type);
        Gold -= actorData.Cost * level;
        InGameManager.ActorManager.SpawnActor(type, tile, Team.CPU, -1, level);
    }
}
