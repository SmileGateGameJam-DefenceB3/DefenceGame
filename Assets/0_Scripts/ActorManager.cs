using System.Collections.Generic;
using UnityEngine;

public class ActorManager
{
    private readonly Dictionary<Team, List<Actor>> _teamActors = new Dictionary<Team, List<Actor>>();
    private readonly Dictionary<ActorType, ActorData> _actorDataList = new Dictionary<ActorType, ActorData>();

    public ActorManager()
    {
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

    public Actor CreatePlacingActor(ActorType actorType, Team team, int direction)
    {
        var actorData = GetActorData(actorType);
        var actor = Object.Instantiate(actorData.Prefab);
        actor.Initialize(actorData, team, direction);
        return actor;
    }

    public Actor SpawnActor(ActorType type, Tile tile, Team team, int direction)
    {
        var actor = CreatePlacingActor(type, team, direction);
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

    public void RemoveActor(Actor actor)
    {
        if (_teamActors.TryGetValue(actor.Team, out var list))
        {
            list.Remove(actor);
        }
    }
}
