using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    NoteManager _noteManager;
    EnemyManager _enemyManager;

    List<Note> _deathSequence = new List<Note>();
    int _complexity;

    public void SetUp(int complexity, NoteManager noteManager, EnemyManager enemyManager)
    {
        _noteManager = noteManager;
        _enemyManager = enemyManager;
        _complexity = Mathf.Clamp(complexity, 1, 4);

        CreateSequence();
        
    }
    public void Print()
    {
        Debug.Log($"Enemy. Complexity: {_complexity}, Death Sequence:");
        foreach( Note note in _deathSequence )
        {
            Debug.Log(note);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyGoal")
        {
            _enemyManager.EnemyHit(_complexity);
            Die();
        }
    }

    void CreateSequence()
    {
        for(int i = 0; i<_complexity; i++)
        {
            _deathSequence.Add(GetRandomNote());
        }

    }
    Note GetRandomNote()
    {
        var notes = _noteManager.Notes;
        return notes[Random.Range(0, notes.Count - 1)];
    }
    

    void Die()
    {
        Destroy(gameObject);
    }
}
