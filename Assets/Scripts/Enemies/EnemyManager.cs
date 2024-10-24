using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] NoteManager _noteManager;

    [SerializeField] GameObject _enemySpawner;
    [SerializeField] GameObject _enemyGoal;

    event Action<int> _onEnemyHit;
    event Action<int> _onEnemyCured;

    public void EnemyHit(int damage) { _onEnemyHit?.Invoke(damage); }
    public event Action<int> OnEnemyHit { add { _onEnemyHit += value; } remove { _onEnemyHit -= value; } }

    public void EnemyCured(int healthPoints) { _onEnemyCured?.Invoke(healthPoints); }
    public event Action<int> OnEnemyCured { add { _onEnemyCured += value; } remove { _onEnemyCured -= value; } }

    float _spawningInterval = 5f;
    const float INTERVAL_ACELERATION_RATE = 0.05f;

    float[] complexityChance = {3f, 8f, 5f, 2f}; // Chances for 1,2,3... complexity (in that order)

    private void Start()
    {
        StartCoroutine(SpawningCoroutine());
    }

    IEnumerator SpawningCoroutine()
    {
        while(true)
        {
            SpawnEnemyAtRandom(RandomCompexity());
            yield return new WaitForSeconds(_spawningInterval);
            _spawningInterval = Mathf.Clamp(_spawningInterval -= INTERVAL_ACELERATION_RATE, 1f, 10);
        }
    }

    int RandomCompexity()
    {
        float chanceSum = complexityChance.Sum();

        float pointer = UnityEngine.Random.Range(0, chanceSum);

        float range = 0;
        for (int i = 0; i < complexityChance.Length; i++)
        {
            range += complexityChance[i];

            if(pointer < range)
            {
                return i + 1;
            }
        }

        return 1; // Never reached.
    }

    GameObject SpawnEnemyAtRandom(int complexity)
    {
        return SpawnEnemy(GetRandomPositionAtSpawn(), complexity);
    }
    GameObject SpawnEnemy(Vector3 pos, int complexity)
    {
        var enemyGO = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().SetUp(complexity, _noteManager, this);
        enemyGO.GetComponent<EnemyController>().SetUp(_enemyGoal);
        return enemyGO;
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
}
