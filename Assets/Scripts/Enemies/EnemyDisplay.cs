using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class EnemyDisplay : MonoBehaviour
{

    [SerializeField] public Canvas Canvas; 
    [SerializeField] private float scaleMultiplier = 3f;

    List<Note> _deathSequence;
    NoteManager _noteManager;

    Image[] _noteImages;


    void Update()
    {
        if (Camera.main != null)
        {
            Canvas.transform.LookAt(Camera.main.transform);
            var newRotation = Quaternion.LookRotation(Camera.main.transform.position - Canvas.transform.position);
            newRotation.y = 0;
            newRotation.z = 0;
            Canvas.transform.rotation = newRotation;
        }
    }

    public void SetUp(List<Note> deathSequence, NoteManager noteManager)
    {
        _deathSequence = deathSequence;
        _noteManager = noteManager;

        _noteManager.OnNoteLogged += HighlightNotes;
        _noteManager.OnBufferReseted += UnHighlightNotes;

        _noteImages = RenderNoteSequence(deathSequence);
    }

    Image[] RenderNoteSequence(List<Note> deathSequence)
    {
        // Configure canvas:
        RectTransform canvasRect = Canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float noteSize = Mathf.Min(canvasWidth / deathSequence.Count, canvasHeight / 2) * scaleMultiplier;

        Image[] images = new Image[deathSequence.Count];

        for (int i = 0; i < deathSequence.Count; i++)
        {
            Note note = deathSequence[i];

            GameObject noteImageObject = new GameObject("NoteImage");
            noteImageObject.transform.SetParent(Canvas.transform);
            noteImageObject.transform.localScale = Vector3.one;

            images[i] = noteImageObject.AddComponent<Image>();
            images[i].sprite = note.Sprite;

            RectTransform rectTransform = images[i].GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(noteSize, noteSize);

            float xPosition = -((canvasWidth / (deathSequence.Count + 1)) * (i + 1) - (canvasWidth / 2));
            float yPosition = 0;

            rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0); 

            rectTransform.localScale = new Vector3(-1, 1, 1);
        }

        return images;
    }

    public void HighlightNotes(Note inputNote)
    {
        // Input note is received only to conform to delegate OnNoteLogged, IT'S NOT USED.

        // Two lists will be used:
        List<Note> noteBuffer = _noteManager.NoteBuffer;    // Notes input by the player.
        List<Note> deathSequence = _deathSequence;          // The enemy's death sequence.

        if(noteBuffer.Count <= 0) return; // If the note buffer is empty return.

        // The note buffer pointer points to the note in the note buffer that is currently being examined.
        // It starts deathSequence.Count positions before the last element in the buffer, but not less than 0:

        int noteBufferPointer = noteBuffer.Count - (deathSequence.Count);
        noteBufferPointer = Mathf.Clamp(noteBufferPointer, 0, int.MaxValue);

        int lastIndexFound = -1;    // Index of the last deathSequence element that was found in the buffer.

        for (int deathSequencePointer = 0; deathSequencePointer < deathSequence.Count; deathSequencePointer++) // For each note in deathSequence:
        {
            // Note buffer pointer will travel throught the note buffer till the end, comparing each element for each note in deathSequence.
            // We only examine the death sequence note if the previous note was found (deathSequencePointer == lastIndexFound+1).
            // The first element will always be examined since lastIndexFound is initialiazed at -1.

            while (noteBufferPointer < noteBuffer.Count && deathSequencePointer == lastIndexFound+1)
            {
                if (_deathSequence[deathSequencePointer] == noteBuffer[noteBufferPointer])                              // If found:
                {
                    _noteImages[deathSequencePointer].sprite = _deathSequence[deathSequencePointer].HighlightSprite;    // Highlight.
                    lastIndexFound = deathSequencePointer;                                                              // Update last index found.
                }

                noteBufferPointer++;

                // NOTE: Next time noteBufferPointer is used it will point to the next element in noteBuffer.
            }
        }
        
    }

    public void UnHighlightNotes()
    {
        for(int i = 0; i < _deathSequence.Count; i++)
        {
            _noteImages[i].sprite = _deathSequence[i].Sprite;
        }
    }

    private void OnDestroy()
    {
        _noteManager.OnNoteLogged -= HighlightNotes;
        _noteManager.OnBufferReseted -= UnHighlightNotes;
    }
}
