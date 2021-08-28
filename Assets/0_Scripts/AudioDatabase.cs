using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

public enum ClipType
{
    InGameBGM,
    
    UIClick,
    Die,
    ItemSpawn,
    LevelUp,
}

[CreateAssetMenu]
public class AudioDatabase : SingletonScriptableObject<AudioDatabase>
{
    [Serializable]
    public class ClipPair
    {
        public ClipType Type;
        public AudioClip Clip;
    }

    [SerializeField] private List<ClipPair> _pairs;
    
    private Dictionary<ClipType, AudioClip> _audioClips;
    
    public AudioClip GetClip(ClipType type)
    {
        if (_audioClips == null)
        {
            _audioClips = new Dictionary<ClipType, AudioClip>();
            foreach (var pair in _pairs)
            {
                _audioClips.Add(pair.Type, pair.Clip);
            }
        }

        return _audioClips[type];
    }
}
