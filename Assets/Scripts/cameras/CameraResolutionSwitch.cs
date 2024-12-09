using UnityEngine;

public class CameraResolutionSwitch : MonoBehaviour
{
    // Coordenadas para orientaci�n vertical
    private Vector3 verticalPosition = new Vector3(-41.25f, 5.06f, 0.95f);
    private Quaternion verticalRotation = Quaternion.Euler(-1.899f, 95.831f, -0.023f);

    // Coordenadas para orientaci�n horizontal
    private Vector3 horizontalPosition = new Vector3(-25f, 1.91f, -0.71f);
    private Quaternion horizontalRotation = Quaternion.Euler(-1.899f, 95.831f, -0.023f);

    // Se usa al inicio para comprobar la orientaci�n de la pantalla
    void Start()
    {
        UpdateCameraPosition();
    }

    // Llamado en cada frame para comprobar la resoluci�n de la pantalla
    void Update()
    {
        UpdateCameraPosition();
    }

    // Funci�n que actualiza la posici�n y rotaci�n de la c�mara seg�n la orientaci�n
    void UpdateCameraPosition()
    {
        // Verificar si la pantalla est� en modo vertical (portrait) o horizontal (landscape)
        if (Screen.height > Screen.width)
        {
            // Si est� en vertical, asigna la posici�n y rotaci�n para la vista vertical
            transform.position = verticalPosition;
            transform.rotation = verticalRotation;
        }
        else
        {
            // Si est� en horizontal, asigna la posici�n y rotaci�n para la vista horizontal
            transform.position = horizontalPosition;
            transform.rotation = horizontalRotation;
        }
    }
}
