using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    NoteManager _noteManager;
    List<Note> _deathSequence = new List<Note>();
    int _complexity;

    public void SetUp(int complexity, NoteManager noteManager)
    {
        _noteManager = noteManager;
        _complexity = Mathf.Clamp(complexity, 1, 4);

        CreateSequence();
        _noteManager.OnNoteLogged += CheckSequence;

        // Renderizar secuencia:
        // Coger sprites de las n notas de death sequence.
        // Crear componentes image con el sprrite y añadirlos al canva.

        Print();
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

    void CheckSequence()
    {
        var noteBuffer = _noteManager.NoteBuffer;

        if (noteBuffer.Count < _complexity) return;

        ;
        for(int myNote = _complexity-1, bufferNote = noteBuffer.Count - 1; myNote >=0; myNote--, bufferNote--)
        {
            if (_deathSequence[myNote] != noteBuffer[bufferNote]) return;
            //Debug.Log("Good");
        }

        Die();
    }

    void Die()
    {
        Debug.Log("The enemy died");
    }
}
