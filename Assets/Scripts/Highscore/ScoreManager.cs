using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManager;
    [SerializeField] NoteManager _noteManager;

    int numEnemiesHealed = 0;

    int combo = 0;
    int totalCombo = 0;

    public int Score { get { return numEnemiesHealed + totalCombo; } }
    public int Combo { get { return combo; } }

    event Action _onScoreChanged;
    public event Action OnScoreChanged { add { _onScoreChanged += value; } remove { _onScoreChanged -= value; } }

    event Action _onComboChanged;
    public event Action OnComboChanged { add {  _onComboChanged += value; } remove { _onComboChanged -= value; } }


    private void Start()
    {
        _enemyManager.OnEnemyCured += (int complexity) =>
        {
            numEnemiesHealed++;
            _onScoreChanged?.Invoke();
        };

        _noteManager.OnNoteLogged += (Note n) =>
        {
            combo++;
            _onComboChanged?.Invoke();
        };

        _noteManager.OnEmptyBeat += () =>
        {
            totalCombo += combo;
            combo = 0;
            _onComboChanged?.Invoke();
            _onScoreChanged?.Invoke();
        };
    }


}
