using System.Collections.Generic;

public class ActorManager
{
    private readonly Dictionary<Team, List<Actor>> _teamActors = new Dictionary<Team, List<Actor>>();
    
    public void PlaceActor(Actor actor, Team team, Tile tile)
    {
        actor.PlaceToTile(tile);
        actor.StartMoveToNextTile();

        if (!_teamActors.TryGetValue(team, out var list))
        {
            list = new List<Actor>();
            _teamActors.Add(team, list);
        }

        list.Add(actor);
    }

    public void RemoveActor(Actor actor)
    {
        if (_teamActors.TryGetValue(actor.Team, out var list))
        {
            list.Remove(actor);
        }
    }
}
