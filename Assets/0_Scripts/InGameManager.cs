﻿using Common;

public class InGameManager : SingletonMonoBehaviour<InGameManager>
{
    public static ActorManager ActorManager => Instance._actorManager;

    private ActorManager _actorManager;
        
    private void Awake()
    {
        _actorManager = new ActorManager();
    }
}