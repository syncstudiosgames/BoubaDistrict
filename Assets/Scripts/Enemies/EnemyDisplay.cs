using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    List<Note> _deathSequence;

    [SerializeField] public Canvas Canvas; 
    [SerializeField] private float scaleMultiplier = 3f; 
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (_deathSequence != null)
        {
            RenderNoteSequence();
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            Canvas.transform.LookAt(mainCamera.transform);
            var newRotation = Quaternion.LookRotation(mainCamera.transform.position - Canvas.transform.position);
            newRotation.y = 0;
            Canvas.transform.rotation = newRotation;
        }
    }

    // Recibo la secuencia
    public void SetSequence(List<Note> deathSequence)
    {
        _deathSequence = deathSequence;
        RenderNoteSequence();
    }

    void RenderNoteSequence()
    {
        RectTransform canvasRect = Canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float noteSize = Mathf.Min(canvasWidth / _deathSequence.Count, canvasHeight / 2) * scaleMultiplier;

        for (int i = 0; i < _deathSequence.Count; i++)
        {
            Note note = _deathSequence[i];
            GameObject noteImageObject = new GameObject("NoteImage");
            noteImageObject.transform.SetParent(Canvas.transform);
            noteImageObject.transform.localScale = Vector3.one;
            Image noteImage = noteImageObject.AddComponent<Image>();
            noteImage.sprite = note.Sprite;

            RectTransform rectTransform = noteImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(noteSize, noteSize);

            float xPosition = (canvasWidth / (_deathSequence.Count + 1)) * (i + 1) - (canvasWidth / 2);
            float yPosition = 0; 

            rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0); 

            rectTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
