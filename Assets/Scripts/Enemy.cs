using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    NoteManager _noteManager;
    List<Note> _deathSequence = new List<Note>();
    int _complexity;
    
    // Canvas para mostrsr las notas
    GameObject sequenceCanvas;
    List<Image> noteImages = new List<Image>();

    public void SetUp(int complexity, NoteManager noteManager)
    {
        _noteManager = noteManager;
        _complexity = Mathf.Clamp(complexity, 1, 4);

        CreateSequence();
        _noteManager.OnNoteLogged += CheckSequence;

        //Renderizar la secuencia
        CreateSequenceCanvas();

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
    void CreateSequenceCanvas()
    {
        sequenceCanvas = new GameObject("NoteSequenceCanvas");
        sequenceCanvas.transform.SetParent(this.transform); 
        Canvas canvas = sequenceCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        RectTransform rectTransform = sequenceCanvas.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(3f, 1f); 

        rectTransform.localPosition = new Vector3(0, 1.5f, 0); 

        float totalWidth = _deathSequence.Count * 0.4f; 
        float startX = -totalWidth / 2; 

        // cada nota en la secuencia crear una imagen la centramos y distribuimos para que mno estén pegadas
        /////SEGUIR PROBANDO
        for (int i = 0; i < _deathSequence.Count; i++)
        {
            GameObject noteGO = new GameObject($"Note_{i}");
            noteGO.transform.SetParent(sequenceCanvas.transform);

            Image noteImage = noteGO.AddComponent<Image>();
            noteImage.sprite = _deathSequence[i].Sprite;

            RectTransform noteRect = noteGO.GetComponent<RectTransform>();
            noteRect.sizeDelta = new Vector2(0.5f, 0.5f);

            noteRect.anchoredPosition = new Vector2(startX + i * 0.4f, 0); 

            noteImages.Add(noteImage);
        }
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

        Destroy(sequenceCanvas); //destruir la secuencia
    }
}
