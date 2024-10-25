using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas : MonoBehaviour
{
    void Start()
    {
        // Ejemplo para ajustar el tamaño de un panel según la resolución
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
