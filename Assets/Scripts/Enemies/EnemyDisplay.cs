using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    NoteManager _noteManager;
    List<Note> _deathSequence;
    int _complexity;

    // Canvas para mostrsr las notas
    GameObject sequenceCanvas;
    List<Image> noteImages = new List<Image>();

    private void Start()
    {
        //Renderizar la secuencia
        CreateSequenceCanvas();
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
}
