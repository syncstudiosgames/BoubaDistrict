using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Credits : https://www.youtube.com/watch?v=gIjajeyjRfE

public class BeatManager : MonoBehaviour
{
    int _bpm;
    AudioSource _audioSource;
    float _errorThreshold = 0.10f;

    

    int _lastExaminedBeat;
    bool _windowOpen = true;
    event Action _openWindowEvent;
    event Action _closeWindowEvent;


    [SerializeField] private Interval[] _intervals;

    public void LoadInfo(int bpm, AudioSource audioSource, float errorThreshold)
    {
        _bpm = bpm; 
        _audioSource = audioSource;
        _errorThreshold = errorThreshold;
    }

    public bool IsOnBeat()
    {
        return _windowOpen;
    }

    void FixedUpdate()
    {
        foreach (var interval in _intervals)
        {
            float clipProgress =    // Clip progress messured in samples (?). Each whole number is a black/beat.
                (
                _audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetBeatLength(_bpm))
                );
            interval.CheckForNewInterval(clipProgress);
        }

        CheckWindow();
    }

    void CheckWindow()
    {
        float beatLength = 60f / _bpm;
        float clipProgress = _audioSource.timeSamples / (_audioSource.clip.frequency * beatLength);

        int closestBeat =(int) Mathf.Round(clipProgress);

        if (closestBeat != _lastExaminedBeat)
        {
            if (!_windowOpen)
            {
                // Check for open window.
                if (clipProgress > closestBeat - _errorThreshold)
                {
                    // Window open.
                    _windowOpen = true;
                    Debug.Log($"Window open! at {clipProgress}");
                }
            }
            else
            {
                // Check for close window.
                if (clipProgress > closestBeat + _errorThreshold)
                {
                    // Wndow closed.
                    _windowOpen = false;
                    _lastExaminedBeat = closestBeat;
                    Debug.Log($"Window closed! at {clipProgress}");
                }
            }
        }
        
    }

}

[System.Serializable]
public class Interval
{
    [SerializeField] private float _numBeats;
    [SerializeField] private UnityEvent _trigger;

    private int _lastBeat = 0;

    public float GetBeatLength(float bpm)
    {
        float _steps = 1 / _numBeats;
        return 60f / (bpm * _steps);
    }

    public void CheckForNewInterval(float clipProgress) // Checks if we have passed a certain beat.
    {
        int currentBeat = Mathf.FloorToInt(clipProgress);

        if (currentBeat != _lastBeat) // Check if the interval has reached a whole number.
        {
            _lastBeat = currentBeat;
            _trigger?.Invoke();
            //Debug.Log(currentBeat);
        }
    }
}
  