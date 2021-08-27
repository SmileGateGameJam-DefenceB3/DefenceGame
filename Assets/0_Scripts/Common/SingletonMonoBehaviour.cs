using System;
using UnityEngine;

namespace Common
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
        {
            var instance = FindObjectOfType<T>();
            if (instance == null)
            {
                instance = new GameObject(nameof(T)).AddComponent<T>();
            }

            return instance;
        });

        public static T Instance => _instance.Value;
    }
}
