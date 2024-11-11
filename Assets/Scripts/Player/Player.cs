using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManager;

    [SerializeField] int _maxHealthPoints;
    int _healthPoints;
    event Action _onHealthValueChange;

    public int MaxHealthPoints { get { return _maxHealthPoints; } }
    public int HealthPoints { get { return _healthPoints; } }
    public event Action OnHealthValueChange { add { _onHealthValueChange += value; } remove { _onHealthValueChange -= value; } }

    public string Name { get; private set; }  // nombre del jugador
    public string GetPlayerName()
    {
        return Name;
    }


    int score;

    event Action _onGameOver;
    public event Action OnGameOver { add { _onGameOver += value; } remove { _onGameOver -= value; } }

    private void Start()
    {
        // Recuperar el nombre del jugador desde PlayerPrefs
        Name = PlayerPrefs.GetString("PlayerName", "Player");  // "Player", por defecto si no se ha definido un nombre

        _healthPoints = _maxHealthPoints;
        _onHealthValueChange?.Invoke();

        _enemyManager.OnEnemyCured += AddHealthPoints;
        _enemyManager.OnEnemyHit += TakeDamage;
    }

    void AddHealthPoints(int healthPoints)
    {
        _maxHealthPoints += healthPoints;
        _healthPoints += healthPoints;
        _onHealthValueChange?.Invoke();
    }

    void RestoreHealthPoints(int healthPoints)
    {
        if (_healthPoints + healthPoints < _maxHealthPoints)
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
        if (_healthPoints - damage > 0)
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
