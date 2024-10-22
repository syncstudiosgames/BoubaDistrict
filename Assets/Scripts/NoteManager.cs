using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class NoteManager : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] List<Note> notes;          // List of existing notes in the game.
    List<Note> noteBuffer = new List<Note>();   // List of notes input by the player on time an consecutively.

    event Action<Note, bool> _onNoteInput;
    public event Action<Note, bool> OnNoteInput { add { _onNoteInput += value; } remove { _onNoteInput -= value; } }

    event Action<Note> _onNoteLogged;
    public event Action<Note> OnNoteLogged { add { _onNoteLogged += value; } remove { _onNoteLogged -= value; } }

    public IReadOnlyList<Note> Notes {  get { return notes.AsReadOnly(); } }
    public IReadOnlyList<Note> NoteBuffer { get { return noteBuffer.AsReadOnly(); } }
    #endregion

    #region Public Methods

    public void InputNote(InputAction action, bool onBeat)
    {
        foreach (var note in notes)
        {
            if (note.Action == action) // Not found or not.
            {
                _onNoteInput?.Invoke(note, onBeat);

                // Check if log note (only if it's on beat):
                if (onBeat)
                {
                    noteBuffer.Add(note);
                    _onNoteLogged?.Invoke(note);
                }
                else
                {
                    ResetBuffer();
                }
                
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
    #endregion

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
