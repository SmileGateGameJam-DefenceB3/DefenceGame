using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private ActorView _view;

    private Tile _laneBeginTile;
    private Tile _targetTile;

    private Tile _currentTile;
    private Tile _nextTile;
    private int _direction;
    private bool _pauseMoving;

    public ActorData Data { get; private set; }
    public ActorView View => _view;

    public float Progress;
    public int Strength => Level * 10 + Data.Grade;
    public int Damage => Data.Grade + Level - 1;
    public int Level { get; private set; }
    public Team Team { get; private set; }
    public float MoveSpeed { get; set; }

    public bool CanLevelUp => Level < Data.MaxLevel;

    public void Initialize(ActorData data, Team team, int direction)
    {
        Data = data;
        MoveSpeed = Constant.Instance.ActorMoveSpeed;
        SetLevel(1, true);
        SetTeam(team);
        SetDirection(direction);
    }

    public void SetLevel(int level, bool isInitial = false)
    {
        Level = level;
        if (!isInitial)
        {
            View.OnLevelChanged(Level);
        }
    }

    public void SetTeam(Team team)
    {
        Team = team;
    }

    public void SetDirection(int direction)
    {
        _direction = direction;
        View.transform.localRotation = direction == -1 ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
    }

    public void Activate()
    {
        _collider.enabled = true;
        View.Activate();
        StartMoveToNextTile();
    }

    public void PlaceToTile(Tile tile)
    {
        transform.position = tile.transform.position;
        _currentTile = tile;
        _nextTile = null;

        StopCoroutine(nameof(MoveToNextTileCo));
    }

    public void StartMoveToNextTile()
    {
        StartCoroutine(nameof(MoveToNextTileCo));
    }

    private IEnumerator MoveToNextTileCo()
    {
        var cursor = _currentTile;
        while (true)
        {
            _targetTile = cursor;
            cursor = cursor.GetAdjacent(_direction, 0);
            if (cursor == null)
            {
                break;
            }
        }

        cursor = _currentTile;
        while (true)
        {
            _laneBeginTile = cursor;
            cursor = cursor.GetAdjacent(-_direction, 0);
            if (cursor == null)
            {
                break;
            }
        }

        while (true)
        {
            _nextTile = _currentTile.GetAdjacent(_direction, 0);
            if (_nextTile == null)
            {
                while (true)
                {
                    float goalX = _currentTile.transform.position.x + _direction * Constant.Instance.TileSize.x / 2f;
                    if (MoveStepTo(goalX))
                    {
                        OnReachEnd();
                        yield break;
                    }

                    yield return null;
                }
            }

            while (true)
            {
                if (_pauseMoving)
                {
                    yield return null;
                }

                if (MoveStepTo(_nextTile.transform.position.x))
                {
                    _currentTile = _nextTile;
                    yield return null;
                    break;
                }

                yield return null;
            }

            bool MoveStepTo(float targetX)
            {
                if (InGameManager.Instance.GameState != GameState.Playing)
                {
                    return false;
                }

                float currentX = transform.position.x;
                currentX += _direction * MoveSpeed * Time.deltaTime;

                if (_direction < 0)
                {
                    currentX = Mathf.Max(currentX, targetX);
                }
                else
                {
                    currentX = Mathf.Min(currentX, targetX);
                }

                var position = transform.position;
                position.x = currentX;
                transform.position = position;

                Progress = Mathf.InverseLerp(_laneBeginTile.transform.position.x, _targetTile.transform.position.x, position.x);

                return currentX == targetX;
            }
        }
    }

    private void OnReachEnd()
    {
        Die(false, false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CheckActorCollide(other))
        {
            return;
        }

        CheckItemCollide(other);
    }

    private bool CheckActorCollide(Collider2D other)
    {
        var otherActor = other.GetComponent<Actor>();
        if (otherActor == null)
        {
            return false;
        }

        if (otherActor.Team == Team)
        {
            return true;
        }

        if (Strength == otherActor.Strength)
        {
            Die();
            otherActor.Die(true, false);
        }
        else if (Strength > otherActor.Strength)
        {
            LevelUp();
            otherActor.Die();
        }
        else
        {
            otherActor.LevelUp();
            Die();
        }

        return true;
    }

    public void LevelUp()
    {
        if (!CanLevelUp)
        {
            return;
        }

        SoundManager.PlaySfx(ClipType.LevelUp);
        SetLevel(Level + 1);
    }

    private void CheckItemCollide(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item == null)
            return;
        item.Func(this);
    }

    private bool CanTakeItem(Item item)
    {
        return true;
    }

    public async void Die(bool playAnimation = true, bool playSound = true)
    {
        if (playSound)
        {
            SoundManager.PlaySfx(ClipType.Die);
        }

        InGameManager.ActorManager.RemoveActor(this);
        _collider.enabled = false;
        StopCoroutine(nameof(MoveToNextTileCo));

        if (playAnimation)
        {
            await View.Die();
        }
        else
        {
            await View.Attack();
            // xd
            InGameManager.Instance.ActorReachedEnd(this);
            await View.FadeOut();
        }

        View.DOKill();
        Destroy(gameObject);
    }
}
