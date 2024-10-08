using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongLoader : MonoBehaviour
{
    [SerializeField] AudioClip _audioClip;
    [SerializeField] int _bpm;
    [SerializeField] float _errorThreshold;

    private void Start()
    {
        var _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        _audioSource.Play();

        var _beatManager = GetComponent<BeatManager>();
        _beatManager.LoadInfo(_bpm, _audioSource, _errorThreshold);
    }
}
