﻿using System.Collections;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private ActorView _view;

    private Tile _currentTile;
    private Tile _nextTile;
    private int _direction;
    private bool _pauseMoving;

    public ActorData Data { get; private set; }
    public ActorView View => _view;

    public int Power { get; private set; }
    public int Level { get; private set; }
    public Team Team { get; private set; }
    public float MoveSpeed { get; set; }

    public void Initialize(ActorData data, Team team, int direction)
    {
        Data = data;
        MoveSpeed = Constant.Instance.ActorMoveSpeed;
        SetLevel(1);
        SetTeam(team);
        SetDirection(direction);
    }

    public void SetLevel(int level)
    {
        Level = level;
        Power = Data.BasePower + (level - 1) * Data.PowerPerLevel;
    }

    public void SetTeam(Team team)
    {
        Team = team;
    }

    public void SetDirection(int direction)
    {
        _direction = direction;
    }

    public void Activate()
    {
        _collider.enabled = true;
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

                return currentX == targetX;
            }
        }
    }

    private void OnReachEnd()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
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
        if (otherActor == null || otherActor.Team == Team)
        {
            return false;
        }

        if (Power == otherActor.Power)
        {
            Die();
            otherActor.Die();
        }
        else if (Power > otherActor.Power)
        {
            otherActor.Die();
        }
        else
        {
            Die();
        }

        return true;
    }

    private void CheckItemCollide(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (item == null)
        {
            return;
        }

        if (CanTakeItem(item))
        {
            item.ApplyEffect(this);
            item.DestroySelf();
        }
    }

    private bool CanTakeItem(Item item)
    {
        return true;
    }

    public void Die()
    {
        InGameManager.ActorManager.RemoveActor(this);
        Destroy(gameObject);
    }
}
