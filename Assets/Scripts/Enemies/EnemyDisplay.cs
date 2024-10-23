using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDisplay : MonoBehaviour
{
    List<Note> _deathSequence;

    [SerializeField] public Canvas Canvas; //canvas del prefab
    private Camera mainCamera;

    private void Start()
    {
        // Inicializar la referencia a la cámara principal
        mainCamera = Camera.main;

        // Cuando esté generada la secuencia renderizo
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
            Canvas.transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - Canvas.transform.position);
        }
    }
    //Recibo la secuencia 
    public void SetSequence(List<Note> deathSequence)
    {
        _deathSequence = deathSequence;
        RenderNoteSequence();
    }

    void RenderNoteSequence()
    {
        // Cojo las dimensiones del canvas  para centrar los sprites
        RectTransform canvasRect = Canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // En funcion del tamaño del canvas y del numero de notas de la sencuencia ajusto el tamaño del sprite
        float noteSize = Mathf.Min(canvasWidth / _deathSequence.Count, canvasHeight / 2);

        for (int i = 0; i < _deathSequence.Count; i++)
        {
            //para cada nota de la sencuencia creo un objeto tipo Image en el canvas y asigno el sprite al Image
            Note note = _deathSequence[i];
            GameObject noteImageObject = new GameObject("NoteImage");
            noteImageObject.transform.SetParent(Canvas.transform);
            noteImageObject.transform.localScale = Vector3.one;
            Image noteImage = noteImageObject.AddComponent<Image>();
            noteImage.sprite = note.Sprite;


            RectTransform rectTransform = noteImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(noteSize, noteSize);

            // Se coje el ancho del canvas y el numero de notas a mostrar y se divide el ancho entre el nº 
            // de notas +1 (para dejar espacio libre entre las notas) se multiplica por i+1 para que cada nota se
            //posicione más a la derecha en función de su indice. Se le resta la mitad del ancho para centrarlas
            float xPosition = (canvasWidth / (_deathSequence.Count + 1)) * (i + 1) - (canvasWidth / 2);
            float yPosition = 0; // para que esten en el medio vertical del canvas (centradas)

            rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0); // Z a 0 para que estén en el plano del canvas


            // Voltear la imagen horizontalmente , si no las letars salen al reves
            rectTransform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
