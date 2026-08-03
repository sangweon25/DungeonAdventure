using System;
using UnityEditor.EditorTools;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField][Range(0f, 1f)] private float _soundEffectVol;
    [SerializeField][Range(0f, 1f)] private float _soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float _musicVol;

    private AudioSource _musicAudioSource;
    public AudioClip musicClip;

    public SoundSource soundSourcePrefab;
    private ObjectPoolManager _poolManager;

    private void Awake()
    {
        instance = this;
        _musicAudioSource = GetComponent<AudioSource>();
        _musicAudioSource.volume = _musicVol;
        _musicAudioSource.loop = true;
    }

    private void Start()
    {
        _poolManager = ObjectPoolManager.Instance;
        ChangeBackGroundMusic(musicClip);
    }

    public void ChangeBackGroundMusic(AudioClip clip)
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = clip;
        _musicAudioSource.Play();
    }

    public static void PlayClip(AudioClip clip)
    {
        GameObject obj = instance._poolManager.GetObject(2, new Vector3(), Quaternion.identity);

        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, instance._soundEffectVol, instance._soundEffectPitchVariance);
    }

}
