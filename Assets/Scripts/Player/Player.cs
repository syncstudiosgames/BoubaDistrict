using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManger;

    [SerializeField] int _maxHealthPoints;
    int _healthPoints;
    event Action _onHealthValueChange;

    public int MaxHealthPoints { get { return _maxHealthPoints; } }
    public int HealthPoints { get { return _healthPoints; } }
    public event Action OnHealthValueChange { add { _onHealthValueChange += value;} remove { _onHealthValueChange -= value; } }


    int score;

    event Action _onGameOver;
    public event Action OnGameOver { add {  _onGameOver += value; } remove { _onGameOver -= value; } }

    private void Start()
    {
        _healthPoints = _maxHealthPoints;
        _onHealthValueChange?.Invoke();

        _enemyManger.OnEnemyCured += AddHealthPoints;
        _enemyManger.OnEnemyHit += TakeDamage;
    }

    void AddHealthPoints(int healthPoints)
    {
        _maxHealthPoints += healthPoints;
        _healthPoints += healthPoints;
        _onHealthValueChange?.Invoke();
    }
    void RestoreHealthPoints(int healthPoints)
    {
        if(_healthPoints+healthPoints < _maxHealthPoints)
        {
            _healthPoints += healthPoints;
        }
        else
        {
            _healthPoints = _maxHealthPoints;
        }

        _onHealthValueChange?.Invoke();
    }
    void TakeDamage(int damage)
    {
        if(_healthPoints - damage > 0)
        {
            _healthPoints -= damage;
        }
        else
        {
            _healthPoints = 0;
            _onGameOver?.Invoke();
        }

        _onHealthValueChange?.Invoke();
    }
}
