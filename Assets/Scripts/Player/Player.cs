using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManager;

    [SerializeField] int _maxHealthPoints;
    int _healthPoints;
    event Action _onHealthValueChange;

    [Header("UI Damage Overlay")]
    [SerializeField] private Image damageOverlay; 
    [SerializeField] private float overlayDuration = 0.5f; 
    [SerializeField] private float overlayMaxAlpha = 0.5f;

    public int MaxHealthPoints { get { return _maxHealthPoints; } }
    public int HealthPoints { get { return _healthPoints; } }
    public event Action OnHealthValueChange { add { _onHealthValueChange += value; } remove { _onHealthValueChange -= value; } }

    public string Name { get; private set; }  // Nombre del jugador
    public int SkinIndex { get; private set; }  // �ndice de la skin seleccionada

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
                    Debug.LogError("El personaje seleccionado no coincide con ning�n prefab en el CharacterSelectionManager.");
                    SkinIndex = 0; // Valor predeterminado
                }
            }
            else
            {
                Debug.LogError("No se encontr� ning�n personaje seleccionado o el array de personajes est� vac�o.");
                SkinIndex = 0; // Valor predeterminado
            }
        }
        else
        {
            Debug.LogError("CharacterSelectionManager.Instance no est� inicializado. Usando �ndice por defecto (0).");
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
        Name = PlayerPrefs.GetString("PlayerName");
        Debug.Log($"UserName: " + Name);

        // Inicializar el �ndice de la skin seleccionada
        SkinIndex = GetSkinIndex();

        // Inicializar los puntos de vida y eventos
        _healthPoints = _maxHealthPoints;
        _onHealthValueChange?.Invoke();

        _enemyManager.OnEnemyCured += AddHealthPoints;
        _enemyManager.OnEnemyHit += TakeDamage;

        if (damageOverlay != null)
        {
            var color = damageOverlay.color;
            color.a = 0;
            damageOverlay.color = color;
        }
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
        TriggerDamageEffect();
    }

    private void TriggerDamageEffect()
    {
        if (damageOverlay != null)
        {
            StartCoroutine(FadeOverlay());
        }
    }

    private IEnumerator FadeOverlay()
    {
        // Aparecer
        float elapsedTime = 0;
        while (elapsedTime < overlayDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, overlayMaxAlpha, elapsedTime / (overlayDuration / 2));
            SetOverlayAlpha(alpha);
            yield return null;
        }

        // Desaparecer
        elapsedTime = 0;
        while (elapsedTime < overlayDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(overlayMaxAlpha, 0, elapsedTime / (overlayDuration / 2));
            SetOverlayAlpha(alpha);
            yield return null;
        }
    }

    private void SetOverlayAlpha(float alpha)
    {
        if (damageOverlay != null)
        {
            var color = damageOverlay.color;
            color.a = alpha;
            damageOverlay.color = color;
        }
    }
}
