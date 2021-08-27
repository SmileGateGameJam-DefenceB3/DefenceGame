using System;
using System.Linq;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T:ScriptableObject
{
    private static readonly Lazy<T> _instance = new Lazy<T>(() =>
    {
        Resources.LoadAll("");
        return Resources.FindObjectsOfTypeAll<T>().First();
    });

    public static T Instance => _instance.Value;
}
