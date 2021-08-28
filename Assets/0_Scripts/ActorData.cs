﻿using UnityEngine;

public enum ActorType
{
    None,
    Rabbit,
    Dog,
    Leopard,
}

[CreateAssetMenu]
public class ActorData : ScriptableObject
{
    public ActorType Type;
    public Actor Prefab;
    public string Name;
    public int BasePower;
    public int MaxLevel;
    public int PowerPerLevel;
}