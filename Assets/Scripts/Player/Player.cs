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

    public string Name { get; private set; }  // Nombre del jugador
    public int SkinIndex { get; private set; }  // Índice de la skin seleccionada

    public string GetPlayerName()
    {
        return Name;
    }

    public int GetSkinIndex()
    {
        if (CharacterSelectionManager.Instance != null)
        {
            GameObject selectedCharacter = CharacterSelectionManager.Instance.GetSelectedCharacter();
            GameObject[] characters = CharacterSelectionManager.Instance.GetCharacters();
            if (selectedCharacter != null && characters != null)
            {
                SkinIndex = Array.IndexOf(characters, selectedCharacter);
                if (SkinIndex == -1)
                {
                    Debug.LogError("El personaje seleccionado no coincide con ningún prefab en el CharacterSelectionManager.");
                    SkinIndex = 0; // Valor predeterminado
                }
            }
            else
            {
                Debug.LogError("No se encontró ningún personaje seleccionado o el array de personajes está vacío.");
                SkinIndex = 0; // Valor predeterminado
            }
        }
        else
        {
            Debug.LogError("CharacterSelectionManager.Instance no está inicializado. Usando índice por defecto (0).");
            SkinIndex = 0;
        }

        return SkinIndex;
    }

    int score;

    event Action _onGameOver;
    public event Action OnGameOver { add { _onGameOver += value; } remove { _onGameOver -= value; } }

    private void Start()
    {
        // Recuperar el nombre del jugador desde PlayerPrefs
        Name = PlayerPrefs.GetString("PlayerName", "Player");

        // Inicializar el índice de la skin seleccionada
        SkinIndex = GetSkinIndex();

        // Inicializar los puntos de vida y eventos
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
