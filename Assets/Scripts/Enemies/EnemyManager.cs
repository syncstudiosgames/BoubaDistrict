using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] NoteManager _noteManager;

    [SerializeField] GameObject _enemySpawner;
    [SerializeField] GameObject _enemyGoal;

    event Action<int> _onEnemyHit;
    event Action<int> _onEnemyCured;

    public void EnemyHit(int damage) { _onEnemyHit?.Invoke(damage); }
    public event Action<int> OnEnemyHit { add {  _onEnemyHit += value; } remove { _onEnemyHit -= value; } }

    public void EnemyCured(int healthPoints) { _onEnemyCured?.Invoke(healthPoints); }
    public event Action<int> OnEnemyCured {  add { _onEnemyCured += value;} remove { _onEnemyCured -= value; } }

    float _spawningInterval = 2f;
    const float INTERVAL_ACELERATION_RATE = 0.05f;

    private void Start()
    {
        StartCoroutine(SpawningCoroutine());

        _onEnemyCured += (int healthPoints) => { Debug.Log("EnemyCured!"); };
        _onEnemyHit += (int damage) => { Debug.Log("EnemyHitYou"); };
    }

    IEnumerator SpawningCoroutine()
    {
        while(true)
        {
            SpawnEnemyAtRandom();
            yield return new WaitForSeconds(_spawningInterval);
            _spawningInterval = Mathf.Clamp(_spawningInterval -= INTERVAL_ACELERATION_RATE, 1f, 10);
        }
    }

    GameObject SpawnEnemyAtRandom()
    {
        return SpawnEnemy(GetRandomPositionAtSpawn());
    }
    GameObject SpawnEnemy(Vector3 pos)
    {
        var enemyGO = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().SetUp(3, _noteManager, this);
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
