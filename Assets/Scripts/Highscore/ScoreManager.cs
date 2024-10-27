using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManager;
    [SerializeField] NoteManager _noteManager;
    [SerializeField] bool _waitForTutorial;

    int _score = 0;

    int numEnemiesHealed = 0;

    int combo = 0;

    public int Score { get { return _score; } set { _score = value; _onScoreChanged?.Invoke(); } }
    public int Combo { get { return combo; } }

    event Action _onScoreChanged;
    public event Action OnScoreChanged { add { _onScoreChanged += value; } remove { _onScoreChanged -= value; } }

    event Action _onComboChanged;
    public event Action OnComboChanged { add {  _onComboChanged += value; } remove { _onComboChanged -= value; } }

    public void StartGame()
    {
        _enemyManager.OnEnemyCured += (int complexity) =>
        {
            numEnemiesHealed++;
            Score += 5 * (int) Math.Pow(complexity,2);
        };

        _noteManager.OnNoteLogged += (Note n) =>
        {
            combo++;
            Score += combo;
            _onComboChanged?.Invoke();
        };

        _noteManager.OnEmptyBeat += () =>
        {
            combo = 0;
            _onComboChanged?.Invoke();
        };

        StartCoroutine(ScorePointEachSecond());
    }

    private void Start()
    {
        if(!_waitForTutorial) { StartGame(); }
    }

    IEnumerator ScorePointEachSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Score += 1;
        }
    }


}
