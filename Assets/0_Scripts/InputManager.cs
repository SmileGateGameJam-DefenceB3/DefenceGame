using Common;
using UnityEngine.EventSystems;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public Tile CurrentHoveredTile { get; private set; }

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
