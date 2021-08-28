using Common;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioSource _audioSource;

    public AudioSource AudioSource => _audioSource;

    public static void PlaySfx(ClipType type)
    {
        Instance._audioSource.PlayOneShot(AudioDatabase.Instance.GetClip(type));
    }

    public static void PlayBGM(ClipType type)
    {
        Instance._audioSource.clip = AudioDatabase.Instance.GetClip(type);
        Instance._audioSource.Play();
    }
}
