using UnityEngine;

public enum ActorType
{
    None,
    Rabbit,
    Leopard,
    Lion,
    Elephant,
}

[CreateAssetMenu]
public class ActorData : ScriptableObject
{
    public ActorType Type;
    public Actor Prefab;
    public string Name;
    public int MaxLevel;
    public int Grade;
    public int Cost;
}
