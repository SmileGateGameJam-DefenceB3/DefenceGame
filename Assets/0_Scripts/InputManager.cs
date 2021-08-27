using Common;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    [SerializeField] private GameObject _tileHoverEffect;

    public Tile CurrentHoveredTile { get; private set; }

    private void LateUpdate()
    {
        if (CurrentHoveredTile != null)
        {
            _tileHoverEffect.SetActive(true);
            _tileHoverEffect.transform.position = CurrentHoveredTile.transform.position;
        }
        else
        {
            _tileHoverEffect.SetActive(false);
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
