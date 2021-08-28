using System;
using Common;
using System.Collections;
using UnityEngine;

public enum GameState
{
    Initialize,
    Playing,
    End,
}

public class InGameManager : SingletonMonoBehaviour<InGameManager>
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Kingdom _playerKingdom;
    [SerializeField] private Kingdom _cpuKingdom;
    
    public static ActorManager ActorManager => Instance._actorManager;
    private ActorManager _actorManager;

    public static TileMap TileMap => Instance._tileMap;
    private TileMap _tileMap;

    public GameState GameState { get; private set; }

    private void Awake()
    {
        _actorManager = new ActorManager();
        _tileMap = FindObjectOfType(typeof(TileMap)) as TileMap;
        _tileMap.Initialize(true);

        _playerKingdom.Initialize();
        _cpuKingdom.Initialize();
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        GameState = GameState.Playing;
        _enemySpawner.StartSpawn();
    }

    public void EndGame()
    {
        _enemySpawner.EndSpawn();
        GameState = GameState.End;
    }

    private Kingdom GetKingdom(Team team)
    {
        return team == Team.Player ? _playerKingdom : _cpuKingdom;
    }

    public void ActorReachedEnd(Actor actor)
    {
        var enemyKingdom = GetKingdom(actor.Team.GetEnemy());
        enemyKingdom.Life -= actor.Damage;
        if (enemyKingdom.Life == 0)
        {
            //
            EndGame();
        }
    }
}
