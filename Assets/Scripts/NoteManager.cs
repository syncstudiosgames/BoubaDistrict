using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class NoteManager : MonoBehaviour
{
    [SerializeField] List<Note> notes;
    List<Note> noteBuffer = new List<Note>();

    event Action _onNoteLogged;
    public event Action OnNoteLogged { add { _onNoteLogged += value; } remove { _onNoteLogged -= value; } }

    public IReadOnlyList<Note> Notes {  get { return notes.AsReadOnly(); } }
    public IReadOnlyList<Note> NoteBuffer { get { return noteBuffer.AsReadOnly(); } }

    public void LogNote(InputAction inputAction)
    {
        foreach (var note in notes)
        {
            if(note.Action == inputAction)
            {
                noteBuffer.Add(note);
                _onNoteLogged?.Invoke();
                return;
            }
        }
        Debug.Log("Error: Note not found.");
    }

    public void ResetBuffer()
    {
        noteBuffer.Clear();
    }

    public void PrintNoteBuffer()
    {
        foreach(var note in noteBuffer)
        {
            Debug.Log(note);
        }
        Debug.Log("................");
    }

}

[System.Serializable]
public class Note
{
    public string noteName;
    public InputActionAsset actions;
    [SerializeField] Sprite _sprite;
    
    [SerializeField] int actionIndex;

    public InputAction Action { get { return actions.actionMaps[0].actions[actionIndex]; } }
    public Sprite Sprite { get { return _sprite; } }

    public override string ToString()
    {
        return noteName;
    }
}
