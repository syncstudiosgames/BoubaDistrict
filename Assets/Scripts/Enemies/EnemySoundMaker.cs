using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemySoundMaker : MonoBehaviour
{
    [SerializeField] bool _playNoiseWhenAppeared;
    [SerializeField] List<AudioClip> _boubaNoises;

    [SerializeField] List<AudioClip> _boubasOnGoalReachedSounds;

    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_playNoiseWhenAppeared) { PlayRandomSound(_audioSource); }
        
    }

    void PlayRandomSound(AudioSource audioSource)
    {
        audioSource.clip = _boubaNoises[UnityEngine.Random.Range(0, _boubaNoises.Count - 1)];
        audioSource.Play();
    }
}
