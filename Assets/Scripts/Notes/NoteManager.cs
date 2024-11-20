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
    [SerializeField] BeatManager _beatManager;
    
    [SerializeField] List<Note> notes;          // List of existing notes in the game.
    List<Note> noteBuffer = new List<Note>();   // List of notes input by the player on time an consecutively.

    bool _noteWasLoggedOnBeat;                  // If a noted was logged during this beat or last beat.

    event Action _onEmptyBeat;
    public event Action OnEmptyBeat { add { _onEmptyBeat += value; } remove { _onEmptyBeat -= value; } }

    event Action<Note, bool> _onNoteInput;
    public event Action<Note, bool> OnNoteInput { add { _onNoteInput += value; } remove { _onNoteInput -= value; } }

    event Action<Note> _onNoteLogged;
    public event Action<Note> OnNoteLogged { add { _onNoteLogged += value; } remove { _onNoteLogged -= value; } }

    event Action _onBufferReseted;
    public event Action OnBufferReseted { add { _onBufferReseted += value; } remove { _onBufferReseted -= value; } }

    public List<Note> Notes {  get { return notes; } }
    public List<Note> NoteBuffer { get { return noteBuffer; } }
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
        _onBufferReseted?.Invoke();
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

    private void Start()
    {
        _beatManager.OnWindowOpen += StartListenningForNote;
        _beatManager.OnWindowClose += FinishListeningForNote;
    }

    // If the player doesn`t input any note we wanna reset the note buffer
    // (as only consecutive notes count for completing a sequence.)
    void StartListenningForNote()
    {
        _noteWasLoggedOnBeat = false; // Reset value.
        OnNoteLogged += ListenForNote;  // Start listenning for note.
    }
    void FinishListeningForNote()
    {
        if (!_noteWasLoggedOnBeat)    // If no note was logger for this beat.
        {
            ResetBuffer();              // Reset note buffer.
            _onEmptyBeat?.Invoke();
        }
        OnNoteLogged -= ListenForNote;  // Stop listenning for note.
    }
    void ListenForNote(Note note)
    {
        _noteWasLoggedOnBeat = true;
    }

}
