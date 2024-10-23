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

    [SerializeField] private EnemyDisplay enemyDisplay;

    public void SetUp(int complexity, NoteManager noteManager, EnemyManager enemyManager)
    {
        _noteManager = noteManager;
        _enemyManager = enemyManager;
        _complexity = Mathf.Clamp(complexity, 1, 4);

        CreateSequence();
        _noteManager.OnNoteLogged += CheckSequence;

        // Pasar la secuencia generada al EnemyDisplay para que la renderice
        if (enemyDisplay != null)
        {
            enemyDisplay.SetSequence(_deathSequence);
        }
        else
        {
            Debug.LogError("No asinado");
        }
    }
    public void Print()
    {
        Debug.Log($"Enemy. Complexity: {_complexity}, Death Sequence:");
        foreach( Note note in _deathSequence )
        {
            Debug.Log(note);
        }
    }

    void CreateSequence()
    {
        for (int i = 0; i < _complexity; i++)
        {
            _deathSequence.Add(GetRandomNote());
        }

    }
    Note GetRandomNote()
    {
        var notes = _noteManager.Notes;
        return notes[Random.Range(0, notes.Count - 1)];
    }
    void CheckSequence(Note inputNote)
    {
        var noteBuffer = _noteManager.NoteBuffer;

        if (noteBuffer.Count < _complexity) return;

        for (int myNote = _complexity - 1, bufferNote = noteBuffer.Count - _complexity; myNote >= 0; myNote--, bufferNote++)
        {
            if (_deathSequence[myNote] != noteBuffer[bufferNote]) return;
        }

        Restore();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyGoal")
        {
            Die();
        }
    }

    void Restore()
    {
        _enemyManager.EnemyCured(_complexity);
        Destroy(gameObject);
    }
    void Die()
    {
        _enemyManager.EnemyHit(_complexity);
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        _noteManager.OnNoteLogged -= CheckSequence;
    }
}
