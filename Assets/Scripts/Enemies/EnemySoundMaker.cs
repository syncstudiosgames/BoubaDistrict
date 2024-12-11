using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundMaker : MonoBehaviour
{


    [SerializeField] bool _playNoiseWhenAppeared;

    [SerializeField] Enemy _enemy;

    [SerializeField] List<AudioClip> _boubaNoises;
    [SerializeField] List<AudioClip> _boubasOnKilledSounds;
    [SerializeField] List<AudioClip> _boubasOnGoalReachedSounds;

    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_playNoiseWhenAppeared) { PlayRandomSound(_boubaNoises, _audioSource); }

        _enemy.OnRestore += () =>
        {
            PlayRandomSound(_boubasOnKilledSounds, _audioSource);
        };

    }

    void PlayRandomSound(List<AudioClip> _audios, AudioSource audioSource)
    {
        audioSource.clip = _audios[UnityEngine.Random.Range(0, _audios.Count)];
        audioSource.Play();
    }
}
