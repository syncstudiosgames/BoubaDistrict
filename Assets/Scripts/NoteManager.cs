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

    public void LogNote(InputAction inputAction)
    {
        foreach (var note in notes)
        {
            if(note.Action == inputAction)
            {
                noteBuffer.Add(note);
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
    
    [SerializeField] int actionIndex;

    public InputAction Action { get { return actions.actionMaps[0].actions[actionIndex]; } }

    public override string ToString()
    {
        return noteName;
    }
}
