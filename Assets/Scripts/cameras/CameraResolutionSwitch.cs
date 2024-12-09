using UnityEngine;

public class CameraResolutionSwitch : MonoBehaviour
{
    // Coordenadas para orientación vertical
    private Vector3 verticalPosition = new Vector3(-41.25f, 5.06f, 0.95f);
    private Quaternion verticalRotation = Quaternion.Euler(-1.899f, 95.831f, -0.023f);

    // Coordenadas para orientación horizontal
    private Vector3 horizontalPosition = new Vector3(-25f, 1.91f, -0.71f);
    private Quaternion horizontalRotation = Quaternion.Euler(-1.899f, 95.831f, -0.023f);

    // Se usa al inicio para comprobar la orientación de la pantalla
    void Start()
    {
        UpdateCameraPosition();
    }

    // Llamado en cada frame para comprobar la resolución de la pantalla
    void Update()
    {
        UpdateCameraPosition();
    }

    // Función que actualiza la posición y rotación de la cámara según la orientación
    void UpdateCameraPosition()
    {
        // Verificar si la pantalla está en modo vertical (portrait) o horizontal (landscape)
        if (Screen.height > Screen.width)
        {
            // Si está en vertical, asigna la posición y rotación para la vista vertical
            transform.position = verticalPosition;
            transform.rotation = verticalRotation;
        }
        else
        {
            // Si está en horizontal, asigna la posición y rotación para la vista horizontal
            transform.position = horizontalPosition;
            transform.rotation = horizontalRotation;
        }
    }
}
