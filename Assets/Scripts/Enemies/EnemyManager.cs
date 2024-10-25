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

    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] NoteManager _noteManager;

    [SerializeField] GameObject _enemySpawner;

    event Action<int> _onEnemyHit;
    event Action<int> _onEnemyCured;

    public void EnemyHit(int damage) { _onEnemyHit?.Invoke(damage); }
    public event Action<int> OnEnemyHit { add { _onEnemyHit += value; } remove { _onEnemyHit -= value; } }

    public void EnemyCured(int healthPoints) { _onEnemyCured?.Invoke(healthPoints); }
    public event Action<int> OnEnemyCured { add { _onEnemyCured += value; } remove { _onEnemyCured -= value; } }

    // Spawning parameters:
    float _spawningInterval = 5f;
    const float SPAWNING_INTERVAL_ACELERATION = 0.05f;
    const float MIN_SPAWNING_INTERVAL = 1f;

    float _enemyMoveSpeed = 15;
    const float MOVE_SPEED_ACELERATION = 0.5f;
    const float MAX_MOVE_SPEED = 30f;

    float[] _complexityChance = {3f, 8f, 5f, 2f}; // Chances for 1,2,3... complexity.
    readonly float[] COMPLEXITY_ACELERATION = { 0.1f, 0.2f, 0.4f, 0.6f };

    #endregion

    #region Private Methods
    void Start()
    {
        StartCoroutine(SpawningCoroutine());
    }

    IEnumerator SpawningCoroutine()
    {
        while(true)
        {
            SpawnEnemyAtRandom(RandomCompexity(), _enemyMoveSpeed);
            yield return new WaitForSeconds(_spawningInterval);
            UpdateSpawningParameters();
        }
    }

    void UpdateSpawningParameters()
    {
        _spawningInterval = ClampLower(_spawningInterval -= SPAWNING_INTERVAL_ACELERATION, MIN_SPAWNING_INTERVAL);
        _enemyMoveSpeed = ClampUpper(_enemyMoveSpeed += MOVE_SPEED_ACELERATION, MAX_MOVE_SPEED);
        _complexityChance = SumFloatArrays(_complexityChance, COMPLEXITY_ACELERATION);
    }
    
    GameObject SpawnEnemyAtRandom(int complexity, float moveSpeed)
    {
        return SpawnEnemy(GetRandomPositionAtSpawn(), complexity, moveSpeed);
    }
    GameObject SpawnEnemy(Vector3 pos, int complexity, float moveSpeed)
    {
        var enemyGO = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().SetUp(complexity, _noteManager, this);
        enemyGO.GetComponent<EnemyController>().SetUp(moveSpeed);
        return enemyGO;
    }

    int RandomCompexity()
    {
        // Segment ballot.
        float chanceSum = _complexityChance.Sum();
        float pointer = UnityEngine.Random.Range(0, chanceSum);

        float range = 0;
        for (int i = 0; i < _complexityChance.Length; i++)
        {
            range += _complexityChance[i];

            if (pointer < range)
            {
                return i + 1;
            }
        }

        return 1; // Never reached.
    }

    Vector3 GetRandomPositionAtSpawn()
    {
        var pos = _enemySpawner.transform.position;
        var scale = _enemySpawner.transform.localScale;

        var x = UnityEngine.Random.Range(pos.x - scale.x / 2, pos.x + scale.x / 2);
        var y = pos.y;
        var z = pos.z;

        return new Vector3 (x, y, z);
    }

    // Aux functions:
    void PrintSpawningParameters()
    {
        string complexityChance = "";
        foreach (float complexity in _complexityChance)
        {
            complexityChance += $"({complexity.ToString()}) ";
        }
        Debug.Log($"Spawning Rate {_spawningInterval}, Enemy Movespeed: {_enemyMoveSpeed}, Complexity Chances: {complexityChance}.");
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
    #endregion
}
