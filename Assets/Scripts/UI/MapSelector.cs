using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public GameObject[] cityObjects; // Array que contiene los objetos de las ciudades
    private int currentIndex = 0; // Índice del objeto actualmente visible

    // Función para cambiar al siguiente mapa
    public void ShowNextCity()
    {
        if (cityObjects.Length == 0) return;

        // Desactivar el mapa actual
        cityObjects[currentIndex].SetActive(false);

        // Incrementar el índice y hacer un bucle si se excede el tamaño del array
        currentIndex = (currentIndex + 1) % cityObjects.Length;

        // Activar el nuevo mapa
        cityObjects[currentIndex].SetActive(true);
    }

    // Función para cambiar al mapa anterior
    public void ShowPreviousCity()
    {
        if (cityObjects.Length == 0) return;

        // Desactivar el mapa actual
        cityObjects[currentIndex].SetActive(false);

        // Decrementar el índice y hacer un bucle si es menor que 0
        currentIndex = (currentIndex - 1 + cityObjects.Length) % cityObjects.Length;

        // Activar el nuevo mapa
        cityObjects[currentIndex].SetActive(true);
    }
}
