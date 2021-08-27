﻿using System.Collections;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private Tile _currentTile;
    private Tile _nextTile;
    private int _direction;
    private bool _pauseMoving;
    
    public float MoveSpeed { get; set; }
    
    public void Initialize(Tile tile, int direction)
    {
        _direction = direction;
        MoveSpeed = Constant.Instance.ActorMoveSpeed;

        PlaceToTile(tile);
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
                break;
            }
            
            while (true)
            {
                if (_pauseMoving)
                {
                    yield return null;
                }
                
                float currentX = transform.position.x;
                float targetX = _nextTile.transform.position.x;

                currentX += _direction * MoveSpeed * Time.deltaTime;

                if (_direction == -1)
                {
                    currentX = Mathf.Max(currentX, targetX);
                }
                else
                {
                    currentX = Mathf.Min(currentX, targetX);
                }

                if (currentX == targetX)
                {
                    break;
                }

                var position = transform.position;
                position.x = currentX;
                transform.position = position;
                
                yield return null;
            }
        }
    }
}
