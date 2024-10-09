using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] NoteManager _noteManager;

    private void Start()
    {
       SpawnEnemy(new Vector3(0, 0, 0));
    }

    void SpawnEnemy(Vector3 pos)
    {
        var enemyGO = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().SetUp(3, _noteManager);
    }
}
