using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManger;

    [SerializeField] int _maxHealthPoints;
    int _healthPoints;

    event Action _onGameOver;
    public event Action OnGameOver { add {  _onGameOver += value; } remove { _onGameOver -= value; } }

    private void Start()
    {
        _healthPoints = _maxHealthPoints;

        _enemyManger.OnEnemyCured += AddHealthPoints;
        _enemyManger.OnEnemyHit += TakeDamage;
    }

    void AddHealthPoints(int healthPoints)
    {
        _maxHealthPoints += healthPoints;
        _healthPoints += healthPoints;
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
    }
}
