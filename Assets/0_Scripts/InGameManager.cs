using Common;
using UI;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Initialize,
    Playing,
    End,
}

public class InGameManager : SingletonMonoBehaviour<InGameManager>
{
    [SerializeField] private Kingdom _playerKingdom;
    [SerializeField] private Kingdom _cpuKingdom;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private ItemRespawn _itemSpawner;
    
    public static ActorManager ActorManager => Instance._actorManager;
    private ActorManager _actorManager;

    public static TileMap TileMap => Instance._tileMap;
    private TileMap _tileMap;

    public GameState GameState { get; private set; }

    private int _gold;

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            OnGoldChanged.Invoke(value);
        }
    }

    public UnityEvent<int> OnGoldChanged = new UnityEvent<int>();

    protected override void Awake()
    {
        base.Awake();

        _actorManager = new ActorManager();
        _tileMap = FindObjectOfType(typeof(TileMap)) as TileMap;
        _tileMap.Initialize(true);

        _playerKingdom.Initialize();
        _cpuKingdom.Initialize();
    }

    private void Start()
    {
        Gold = Constant.Instance.MaxGold;

        if (!SoundManager.Instance.AudioSource.isPlaying)
        {
            SoundManager.PlayBGM(ClipType.InGameBGM);
        }

        _tutorial.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        Actor.Id = 0;
        GameState = GameState.Playing;

        if (!GameSetting.Instance.TestEnemyPlace)
        {
            AmazingAIScript.Instance.Run();
        }

        _itemSpawner.StartSpawn();
    }

    private void Update()
    {
        if (GameState == GameState.Playing)
        {
            if (ActorManager.GetActorCount() == 0)
            {
                if (Gold == 0 && AmazingAIScript.Instance.Gold == 0)
                {
                    EndGame(Result.Draw);
                }
            }
        }
    }

    public void EndGame(Result result)
    {
        AmazingAIScript.Instance.Stop();
        GameState = GameState.End;

        InGameUIManager.Instance.GameOverScreen.Open(result);
    }

    public Kingdom GetKingdom(Team team)
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
            EndGame(actor.Team == Team.Player ? Result.Win : Result.Lost);
        }
    }
}
