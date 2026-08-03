using System;
using UnityEngine;

public class SoundSource : MonoBehaviour , IPoolable
{
    private AudioSource _audioSource;
    private Action<GameObject> _returnToPool;

    public void Play(AudioClip clip, float soundEffectVol, float soundEffectPitchVariance)
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        CancelInvoke();
        _audioSource.clip = clip;
        _audioSource.volume = soundEffectVol;
        _audioSource.Play();
        _audioSource.pitch = 1f + UnityEngine.Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        Invoke("Disable", clip.length + 2);
    }

    public void Disable()
    {
        _audioSource.Stop();
        OnDeSpawn();
        //Destroy(this.gameObject);
    }


    public void Initialize(Action<GameObject> returnAction)
    {
        _returnToPool = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDeSpawn()
    {
        _returnToPool?.Invoke(gameObject);
    }
}
