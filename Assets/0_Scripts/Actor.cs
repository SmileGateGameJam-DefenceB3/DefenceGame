using System.Collections;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private ActorView _view;
    
    private Tile _currentTile;
    private Tile _nextTile;
    private int _direction;
    private bool _pauseMoving;
    
    public Team Team { get; private set; }
    public float MoveSpeed { get; set; }
    public ActorView View => _view;
    
    public void Initialize(int direction, Team team)
    {
        _direction = direction;
        MoveSpeed = Constant.Instance.ActorMoveSpeed;
        Team = team;
    }

    public void Activate()
    {
        _collider.enabled = true;
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
                yield break;
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
                
                if (currentX == targetX)
                {
                    _currentTile = _nextTile;
                    yield return null;
                    break;
                }
                
                yield return null;
            }
        }
    }
}
