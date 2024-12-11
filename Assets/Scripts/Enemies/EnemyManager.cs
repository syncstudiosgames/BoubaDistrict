using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyManager : MonoBehaviour
{
    #region Variables & Properties

    [SerializeField] List<GameObject> _enemiesPrefabs;
    [SerializeField] List<GameObject> _enemyBossesPrefabs;

    [SerializeField] NoteManager _noteManager;

    [SerializeField] GameObject _enemySpawner;

    [SerializeField] bool _waitForTutorial;
    [SerializeField] bool _allowCeroEnemies;
    [SerializeField] bool _renderEnemyUI;

    public void AllowCeroEnemies(bool allowCeroEnemies) { _allowCeroEnemies = allowCeroEnemies; }

    Vector3 _lastSpawnningPosition;
    [SerializeField] float _notSpawningBracketLength = 7;

    bool _bossSpawned;

    int _currentNumOfEnemies = 0;
    event Action _currentNumEnemiesOnValueChange;
    public int CurrentNumOfEnemies { get { return _currentNumOfEnemies; } private set { _currentNumOfEnemies = value; _currentNumEnemiesOnValueChange?.Invoke(); } }
    public event Action CurrentNumEnemiesOnChangeValue { add { _currentNumEnemiesOnValueChange += value; } remove { _currentNumEnemiesOnValueChange -= value; } }

    // Events:
    event Action _onEnemySpawned;
    event Action<int> _onEnemyHit;
    event Action<int> _onEnemyCured;

    public event Action OnEnemySpawned {  add { _onEnemySpawned += value;} remove { _onEnemySpawned -= value; } }
    public void EnemyHit(int damage) { _onEnemyHit?.Invoke(damage); }
    public event Action<int> OnEnemyHit { add { _onEnemyHit += value; } remove { _onEnemyHit -= value; } }

    public void EnemyCured(int healthPoints) { _onEnemyCured?.Invoke(healthPoints); }
    public event Action<int> OnEnemyCured { add { _onEnemyCured += value; } remove { _onEnemyCured -= value; } }

    public void BossGone() { _bossSpawned = false; }

    // Spawning parameters:
    float _dificultyStep = 0.01f;
    float _dificultyStepAcceleration = 0.01f;

    float _baseSpawningTime = 4.5f;
    const float MIN_SPAWNING_INTERVAL = 2f;

    float _baseMoveSpeed = 10;
    const float MAX_MOVE_SPEED = 20f;

    float[] _baseComplexityChance = {3f, 9f, 2f, 0.01f}; // Chances for 1,2,3... complexity.
    readonly float[] MAX_COMPLEXITY_CHANCES = { 1f, 2f, 8f, 6f };

    float[] _typeOfEnemyChanche = { 10f, 1f };

    

    #endregion

    #region Public Methods

    public void StartGame()
    {
        
        Invoke("StartSpawning", 0);
    }

    public void StartGAmeWithDelay(float delay)
    {
        Invoke("StartSpawning", delay);
        
    }
    #endregion

    #region Private Methods

    private void Start()
    {
        OnEnemySpawned += () => { CurrentNumOfEnemies += 1; };
        OnEnemyHit += (int damage) => { CurrentNumOfEnemies -= 1; };
        OnEnemyCured += (int healthPoints) => { CurrentNumOfEnemies -= 1; };
        CurrentNumEnemiesOnChangeValue += () => { CheckFor0Enemies(); };


        if(!_waitForTutorial) { StartSpawning(); }
    }
    void StartSpawning()
    {
        StartCoroutine(SpawningCoroutine());
    }

    IEnumerator SpawningCoroutine()
    {
        while(true)
        {
            SpawnEnemyRandom(); // SpawnEnemy.

            // Increment dificulty step. NOTE: Dificulty step is between 0 and 1.
            _dificultyStep = (_dificultyStep + _dificultyStepAcceleration < 1) ? 
                _dificultyStep + _dificultyStepAcceleration : 1;

            // Calculate new time interval using logaritmic interpolation:
            var spawningTime = interpolateValueLog(_dificultyStep, _baseSpawningTime, MIN_SPAWNING_INTERVAL);

            yield return new WaitForSeconds(spawningTime);  // Wait said interval.
        }
    }
    
    GameObject SpawnEnemyRandom()
    {
        var enemyPrefab = _enemiesPrefabs[GetRandomIndexByChance(_typeOfEnemyChanche)];

        Vector3 position;
        if (!_bossSpawned)
        {
            position = GetRandomPositionAtSpawn();
        }
        else
        {
            position = GetRandomPositionAtSpawnSides();
        }
        

        //Debug.Log(string.Join(", ", InterpolateArrayLog(_dificultyStep, _baseComplexityChance, MAX_COMPLEXITY_CHANCES)));
        var complexityChances = InterpolateArrayLog(_dificultyStep, _baseComplexityChance, MAX_COMPLEXITY_CHANCES);
        var complexity = GetRandomIndexByChance(complexityChances) + 1;   // Complexity 1 is in index 0, 2 in 1, and so on...

        var moveSpeed = interpolateValueLog(_dificultyStep, _baseMoveSpeed, MAX_MOVE_SPEED);

        return SpawnEnemy(enemyPrefab, position, complexity, moveSpeed, 1, _renderEnemyUI);
    }
    GameObject SpawnEnemy(GameObject prefab, Vector3 pos, int complexity, float moveSpeed, int lives = 1, bool renderSequence = true)
    {
        var enemyGO = Instantiate(prefab, pos, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().SetUp(complexity, moveSpeed, _noteManager, this, lives, renderSequence);
        _onEnemySpawned?.Invoke();
        return enemyGO;
    }
    public GameObject SpawnEnemyBoss(int bossIndex = 0)
    {
        _bossSpawned = true;

        var pos = GetCenterPosAtSpawn();
        return SpawnEnemy(_enemyBossesPrefabs[bossIndex], pos, 4, 5, 3, _renderEnemyUI);
    }
    public void SpawnSimpleEnemy()
    {
        SpawnEnemy(_enemiesPrefabs[0], GetRandomPositionAtSpawn(), 1, 10, 1, false);
    }
    public void SpawnEnemyWithComplexity(int complexity)
    {
        SpawnEnemy(_enemiesPrefabs[0], GetRandomPositionAtSpawn(), complexity, 10);
    }

    void CheckFor0Enemies()
    {
        if (_allowCeroEnemies) return;
        if (_currentNumOfEnemies == 0) SpawnEnemyRandom();
    }

    #endregion

    #region Utility Methods
    int GetRandomIndexByChance(float[] chances)
    {
        // Segment ballot.
        float chanceSum = chances.Sum();
        float pointer = UnityEngine.Random.Range(0, chanceSum);

        float range = 0;
        for (int i = 0; i < chances.Length; i++)
        {
            range += chances[i];

            if (pointer < range)
            {
                return i;
            }
        }

        return 1; // Never reached.
    }
    Vector3 GetRandomPositionAtSpawn()
    {
        var spawnerPos = _enemySpawner.transform.position;
        var spawnerScale = _enemySpawner.transform.localScale;


        var leftBound = spawnerPos.x - spawnerScale.x / 2;
        var rightBound = spawnerPos.x + spawnerScale.x / 2;

        float x;

        if(_lastSpawnningPosition == null)
        {
            x = UnityEngine.Random.Range(leftBound, rightBound);
        }
        else
        {
            bool pickLeft = false;
            bool sidePicked = false;

            var lastSpawningX = _lastSpawnningPosition.x;

            var leftX = lastSpawningX - _notSpawningBracketLength / 2;
            if(leftX < leftBound) { pickLeft = false; sidePicked = true; }

            var rightX = lastSpawningX + _notSpawningBracketLength / 2;
            if (rightX > rightBound) { pickLeft = true; sidePicked = true; }

            if(!sidePicked) pickLeft = UnityEngine.Random.value < 0.5f;

            x = pickLeft? 
                UnityEngine.Random.Range(spawnerPos.x - spawnerScale.x / 2, leftX)
                : 
                UnityEngine.Random.Range(rightX, spawnerPos.x + spawnerScale.x / 2);
        }

        _lastSpawnningPosition = new Vector3(x, spawnerPos.y, spawnerPos.z);

        return _lastSpawnningPosition;
    }
    Vector3 GetCenterPosAtSpawn()
    {
        var spawnerPos = _enemySpawner.transform.position;
        var spawnerScale = _enemySpawner.transform.localScale;

        return spawnerPos;
    }
    Vector3 GetRandomPositionAtSpawnSides()
    {
        float x;

        float spawningDistanceAllowFromSide = 2f;

        var spawnerPos = _enemySpawner.transform.position;
        var spawnerScale = _enemySpawner.transform.localScale;
        var leftBound = spawnerPos.x - spawnerScale.x / 2;
        var rightBound = spawnerPos.x + spawnerScale.x / 2;

        var leftX = leftBound + spawningDistanceAllowFromSide;
        var rightX = rightBound - spawningDistanceAllowFromSide;

        var pickLeft = UnityEngine.Random.value < 0.5f;
        x = pickLeft ?
            UnityEngine.Random.Range(leftBound, leftX)
            :
            UnityEngine.Random.Range(rightX, rightBound);

        _lastSpawnningPosition = new Vector3(x, spawnerPos.y, spawnerPos.z);
        return _lastSpawnningPosition;
    }

    void PrintSpawningParameters()
    {
        string complexityChance = "";
        foreach (float complexity in _baseComplexityChance)
        {
            complexityChance += $"({complexity.ToString()}) ";
        }
        Debug.Log($"Spawning Rate {_baseSpawningTime}, Enemy Movespeed: {_baseMoveSpeed}, Complexity Chances: {complexityChance}.");
    }
    float ClampUpper(float value, float max) { return value > max ? max : value; }
    float ClampLower(float value, float min) { return value < min ? min : value; }
    float[] SumFloatArrays(float[] array1, float[] array2)
    {
        int length = Math.Min(array1.Length, array2.Length);
        float[] result = new float[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = array1[i] + array2[i];
        }
        return result;
    }
    float interpolateValueLog(float value, float min, float max)
    {

        // Validar los valores de entrada
        if (value < 0.0f || value > 1.0f) throw new ArgumentOutOfRangeException("Value must be in [0, 1].");
        if (min <= 0.0f || max <= 0.0f) throw new ArgumentOutOfRangeException("min / max must be > 0.");

        // Calcular los logaritmos de min y max
        float logMin = (float)Math.Log(min);
        float logMax = (float)Math.Log(max);

        // Interpolación lineal en el espacio logarítmico
        float interpolatedLog = logMin + value * (logMax - logMin);

        // Volver al espacio original exponenciando el valor interpolado
        return (float)Math.Exp(interpolatedLog);
    }
    float[] InterpolateArrayLog(float value, float[] min, float[] max)
    {
        if (min.Length > max.Length)
            throw new ArgumentOutOfRangeException("max.Length must be greater than or equal to min.Length.");

        var interpolatedArray = new float[min.Length];

        for(int i = 0; i < min.Length; i++)
        {
            interpolatedArray[i] = interpolateValueLog(value, min[i], max[i]);
        }

        return interpolatedArray;
    }

    #endregion
}
