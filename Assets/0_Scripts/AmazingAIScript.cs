using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AmazingAIScript : SingletonMonoBehaviour<AmazingAIScript>
{
    private readonly List<Tile> _availableTiles = new List<Tile>();
    private float _nextSpawnTime;
    private readonly Queue<Tile> _actQueue = new Queue<Tile>();

    private StageData.Stage _stage;
    private int _rabbitRushCount = 2;

    public const int MaxGold = 150;

    private int _gold;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = Mathf.Clamp(value, 0, MaxGold);
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

        _stage = InGameManager.Instance.CurrentStage;
        Gold = _stage.CPUGold;
    }

    public void Run()
    {
        StartCoroutine(nameof(RunCo));
    }

    public void Stop()
    {
        StopCoroutine(nameof(RunCo));
    }

    private bool did;

    private void Update()
    {
        if (did || !_stage.IsHard)
        {
            return;
        }

        if (InGameManager.Instance.GetKingdom(Team.CPU).Life <= 12)
        {
            did = true;
            SpawnRandom(InGameManager.TileMap[Random.Range(6, 8), 0], Random.Range(1, 4));
            SpawnRandom(InGameManager.TileMap[Random.Range(6, 8), 1], Random.Range(1, 4));
            SpawnRandom(InGameManager.TileMap[Random.Range(6, 8), 2], Random.Range(1, 4));
            SpawnRandom(InGameManager.TileMap[Random.Range(6, 8), 3], Random.Range(1, 4));
            SpawnRandom(InGameManager.TileMap[Random.Range(6, 8), 4], Random.Range(1, 4));
        }
    }

    readonly Dictionary<int, int> cpuDict = new Dictionary<int, int>();
    readonly Dictionary<int, int> playerDict = new Dictionary<int, int>();

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
                _nextSpawnTime = Time.time + Random.Range(_stage.MinCPUSpawnDelay, _stage.MaxCPUSpawnDelay);

                if (_rabbitRushCount > 0 && Random.Range(0, 1f) < 0.1f)
                {
                    _rabbitRushCount--;
                    int rabbitCount = Random.Range(6, 9) + _stage.Factor * 5;
                    yield return HorizontalRabbitRush(rabbitCount, GetRandomTile());
                }
                else
                {
                    if (_stage.IsHard)
                    {
                        void Fill(Dictionary<int, int> target, List<Actor> actors)
                        {
                            target.Clear();
                            foreach (var actor in actors)
                            {
                                if (!target.TryGetValue(actor.Y, out var oldValue) || actor.Strength > oldValue)
                                {
                                    target[actor.Y] = actor.Strength;
                                }
                            }
                        }

                        Fill(cpuDict, InGameManager.ActorManager.GetActors(Team.CPU));
                        Fill(playerDict, InGameManager.ActorManager.GetActors(Team.Player));

                        foreach (var cpu in cpuDict)
                        {
                            if (playerDict.TryGetValue(cpu.Key, out var playerStrength))
                            {
                                if (playerStrength <= cpu.Value)
                                {
                                    playerDict.Remove(cpu.Key);
                                }
                            }
                        }

                        if (playerDict.Count == 0)
                        {
                            int level = GetRandomLevel(80, 13, 4, 3 + _stage.Factor * 5);
                            SpawnRandom(GetRandomTile(), level);
                        }
                        else
                        {
                            var target = playerDict.First();
                            foreach (var pair in playerDict)
                            {
                                if (pair.Value > target.Value)
                                {
                                    target = pair;
                                }
                            }

                            int y = target.Key;
                            int strength = target.Value;

                            int level = strength / 10;
                            int grade = (strength - level * 10) + 1;

                            grade = Mathf.Clamp(grade, (int) ActorType.Rabbit, (int) ActorType.Elephant);
                            Spawn((ActorType) grade, level, InGameManager.TileMap[7, y]);
                        }
                    }
                    else
                    {
                        var defenceTile = GetDefenceTile();
                        if (defenceTile == null || Random.Range(0, 1f) < 0.05f)
                        {
                            SpawnRandom(GetRandomTile(), GetRandomLevel(30, 30, 20 + _stage.Factor * 10, 5 + _stage.Factor * 10));
                        }
                        else
                        {
                            int level = GetRandomLevel(80, 13, 4, 3 + _stage.Factor * 5);
                            SpawnRandom(defenceTile, level);
                        }
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
            yield return new WaitForSeconds(0.2f);
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
