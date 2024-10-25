using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteDisplay : MonoBehaviour
{
    [SerializeField] NoteManager _noteManager;

    [SerializeField] float _imageWidth;
    [SerializeField] float _imageHeight;


    private void Start()
    {
        _noteManager.OnNoteInput += DisplayNote;
    }

    void DisplayNote(Note note, bool onBeat)
    {
        // Create and set up GameObject:
        var noteGO = new GameObject("NoteImage", typeof(RectTransform));
        noteGO.transform.SetParent(transform);

        var rectTransform = noteGO.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.sizeDelta = new Vector2(_imageWidth, _imageHeight);

        // Create image and assign sprite:
        var image = noteGO.AddComponent<Image>();
        image.sprite = note.Sprite;

        // Animation:
        rectTransform.LeanMoveLocal(new Vector2(rectTransform.localPosition.x, rectTransform.localPosition.y + 100), 0.2f).setEaseInOutQuart(); // Animate position.
        rectTransform.LeanScale((Vector2)transform.localScale + Vector2.one, 0.2f);                                                             // Animate scale.

        LeanTween.value(noteGO, 1f, 0f, 0.2f).setOnUpdate((float value) =>                                                                      // Animate alpha.
        {
            Color color = image.color;
            color.a = value;
            image.color = color;
        }).setOnComplete(() =>                                                                                                                  // Destroy GO when the animation is done.
        {
            Destroy(noteGO);
        });

    }
}
