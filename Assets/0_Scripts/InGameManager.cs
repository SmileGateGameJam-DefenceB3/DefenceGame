using Common;
using System.Collections;
using UnityEngine;
public class InGameManager : SingletonMonoBehaviour<InGameManager>
{
    public static ActorManager ActorManager => Instance._actorManager;
    private ActorManager _actorManager;

    private void Awake()
    {
        _actorManager = new ActorManager();
        var tileMap = FindObjectOfType(typeof(TileMap)) as TileMap;
        tileMap.Initialize(true);
    }
}
