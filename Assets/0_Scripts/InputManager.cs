using Common;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private TileHoverEffect _tileHoverEffect;
    
    public Tile CurrentHoveredTile { get; private set; }

    private void Awake()
    {
        _tileHoverEffect = FindObjectOfType<TileHoverEffect>();
        _tileHoverEffect.SetColor(Color.yellow);
    }

    private void LateUpdate()
    {
        if (CurrentHoveredTile != null)
        {
            _tileHoverEffect.gameObject.SetActive(true);
            _tileHoverEffect.transform.position = CurrentHoveredTile.transform.position;

            if (CurrentHoveredTile.Coord.x >= Constant.Instance.MapSize.x / 2)
            {
                _tileHoverEffect.SetColor(Color.red);
            }
            else
            {
                _tileHoverEffect.SetColor(Color.yellow);
            }
        }
        else
        {
            _tileHoverEffect.gameObject.SetActive(false);
        }
    }

    public void ReportEnter(Tile tile)
    {
        CurrentHoveredTile = tile;
    }

    public void ReportExit(Tile tile)
    {
        if (CurrentHoveredTile == tile)
        {
            CurrentHoveredTile = null;
        }
    }
}
