using System;
using System.Collections;
using System.Collections.Generic;
using Code.Enums;
using Kuhpik;
using UnityEngine;

[Serializable]
public class AudioData
{
    [field: SerializeField] public AudioTypes AudioType { get; private set; }
    [field: SerializeField] public AudioClip AudioClip { get; private set; }
    [field: SerializeField] public float VolumeAudio { get; private set; }
    public AudioSource AudioSource { get; set; }
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private List<AudioData> _audioData = new List<AudioData>();
    [SerializeField] private AudioSource _audioSource;
    private List<AudioSource> _effects = new List<AudioSource>();

    private float _effectsVolume = 1;

    private void Start()
    {
        foreach (var audio in _audioData)
        {
            GameObject instance = new GameObject();
            AudioSource audioSource = instance.AddComponent<AudioSource>();
            audioSource.volume = _effectsVolume;
            audio.AudioSource = audioSource;

            _effects.Add(audioSource);
        }
    }

    public void SoundVolume(float value)
    {
        _audioSource.volume = value;
    }

    public void EffectsVolume(float value)
    {
        for (int i = 0; i < _effects.Count; i++)
        {
            _effectsVolume = value;
            _effects[i].volume = value;
        }
    }

    public void PlayAudio(AudioTypes audioType)
    {
        AudioData audioData = _audioData.Find(x => x.AudioType == audioType);
        audioData.AudioSource.PlayOneShot(audioData.AudioClip);
    }
}
