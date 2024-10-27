using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Credits : https://www.youtube.com/watch?v=gIjajeyjRfE

public class BeatManager : MonoBehaviour
{
    [SerializeField] bool _waitForTutorial;

    #region Variables & Properties
    int _bpm;
    AudioSource _audioSource;
    float _errorThreshold = 0.10f;

    int _lastBeat = 0;
    event Action _onBeat;
    public event Action OnBeat { add { _onBeat += value; } remove { _onBeat -= value; } }

    int _lastExaminedBeat;
    bool _windowOpen = true;    // Window of time when the user can input a note on beat.

    event Action _onWindowOpen;
    event Action _onWindowClose;

    public event Action OnWindowOpen { add { _onWindowOpen += value; } remove { _onWindowOpen -= value; } }
    public event Action OnWindowClose { add {  _onWindowClose += value; } remove { _onWindowClose -= value; } }

    [SerializeField] private Interval[] _intervals; // Each represents an n beats duration interval.

    public float ClipProgress { get { return _audioSource.timeSamples / (_audioSource.clip.frequency * (60f / _bpm)); } } // Each whole numbe is a beat.
                                                                                                                          // Playback position in PCM samples / (frequency * beat length)
    #endregion

    #region Public Methods
    public void LoadInfo(int bpm, AudioSource audioSource, float errorThreshold)
    {
        _bpm = bpm; 
        _audioSource = audioSource;
        _errorThreshold = errorThreshold;
    }

    public void StartPlaying()
    {
        _audioSource.Play();
    }

    public bool IsOnBeat()
    {
        return _windowOpen;
    }
    #endregion

    #region Private Methods

    private void Start()
    {
        if (!_waitForTutorial) { StartPlaying(); }
    }
    void FixedUpdate()
    {
        // MAIN BEAT EVENT:
        int currentBeat = Mathf.FloorToInt(ClipProgress);
        if (currentBeat != _lastBeat) // Check if the interval has reached a whole number.
        {
            _lastBeat = currentBeat;
            _onBeat?.Invoke();
        }

        // BEAT EVENTS: Fire events each beat (different events for different intervals (1 beat, 2 beats, etc.)
        foreach (var interval in _intervals)
        {
            // For each interval check if it's own clip progress...
            float clipProgress =    
                (
                _audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetBeatLength(_bpm))
                );
            //  ... has reached a whole number (beat)...
            interval.CheckForBeat(clipProgress);
            // ... if so, trigger an event.
        }

        // BEAT WINDOW: Check clip progress to open a window for the user to input notes.
        CheckWindow();
    }

    void CheckWindow()
    {
        // Given clip progress
        // 1. Identify the closest beat (each whole number is a beat).
        // 2. Open window at closestBeat - _errorThreshold.
        // 3. Close window at closestBeat + _errorThreshold.
        // 4. When closing window take the recent examined beat (closestBeat) as the last examined beat.

        int closestBeat =(int) Mathf.Round(ClipProgress);

        if (closestBeat != _lastExaminedBeat) // If the beat was not already examined.
        {
            if (!_windowOpen)
            {
                // Check for window openning.
                if (ClipProgress > closestBeat - _errorThreshold)
                {
                    // Window open.
                    _windowOpen = true;
                    _onWindowOpen?.Invoke();
                }
            }
            else
            {
                // Check for window closing.
                if (ClipProgress > closestBeat + _errorThreshold)
                {
                    // Wndow closed.
                    _windowOpen = false;
                    _lastExaminedBeat = closestBeat;
                    _onWindowClose?.Invoke();
                }
            }
        }
        
    }
    #endregion

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

    public void CheckForBeat(float clipProgress) // Checks if a beat was reached, if so triggers an event.
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
  