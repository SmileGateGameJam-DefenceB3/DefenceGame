using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    private readonly Dictionary<Team, List<Actor>> _teamActors = new Dictionary<Team, List<Actor>>();
    private readonly Dictionary<ActorType, ActorData> _actorDataList = new Dictionary<ActorType, ActorData>();

    public ActorManager()
    {
        _teamActors.Add(Team.Player, new List<Actor>());
        _teamActors.Add(Team.CPU, new List<Actor>());
        
        var actorDataList = Resources.LoadAll("Actors");
        foreach (var data in actorDataList)
        {
            var actorData = data as ActorData;
            if (actorData != null)
            {
                _actorDataList.Add(actorData.Type, actorData);
            }
        }
    }

    public ActorData GetActorData(ActorType type) => _actorDataList[type];

    public int GetActorCount()
    {
        int count = 0;
        foreach (var team in _teamActors.Values)
        {
            count += team.Count;
        }

        return count;
    }
    
    public Actor CreatePlacingActor(ActorType actorType, Team team, int direction, int level = 1)
    {
        var actorData = GetActorData(actorType);
        var actor = Object.Instantiate(actorData.Prefab);
        actor.Initialize(actorData, team, direction, level);
        return actor;
    }

    public Actor SpawnActor(ActorType type, Tile tile, Team team, int direction, int level = 1)
    {
        var actor = CreatePlacingActor(type, team, direction, level);
        SpawnActor(actor, tile);
        return actor;
    }
    
    public void SpawnActor(Actor actor, Tile tile)
    {
        actor.PlaceToTile(tile);
        actor.Activate();

        if (!_teamActors.TryGetValue(actor.Team, out var list))
        {
            list = new List<Actor>();
            _teamActors.Add(actor.Team, list);
        }

        list.Add(actor);
    }

    public List<Actor> GetActors(Team team)
    {
        return _teamActors[team];
    }

    public void RemoveActor(Actor actor)
    {
        if (_teamActors.TryGetValue(actor.Team, out var list))
        {
            list.Remove(actor);
        }
    }
}
