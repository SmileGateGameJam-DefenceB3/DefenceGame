using Common;
using UnityEngine.EventSystems;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    public Tile CurrentHoveredTile { get; private set; }
    
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            CurrentHoveredTile = null;
            return;
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
