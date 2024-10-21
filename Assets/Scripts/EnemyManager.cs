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

    private void Start()
    {
        SpawnEnemy(GetRandomPositionAtSpawn());
    }

    void SpawnEnemy(Vector3 pos)
    {
        var enemyGO = Instantiate(_enemyPrefab, pos, Quaternion.identity);

        enemyGO.GetComponent<Enemy>().SetUp(3, _noteManager);
    }

    Vector3 GetRandomPositionAtSpawn()
    {
        var pos = _enemySpawner.transform.position;
        var scale = _enemySpawner.transform.localScale;

        var x = Random.Range(pos.x - scale.x / 2, pos.x + scale.x / 2);
        var y = pos.y;
        var z = pos.z;

        return new Vector3 (x, y, z);
    }
}
