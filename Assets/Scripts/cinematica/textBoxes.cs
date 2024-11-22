using UnityEngine;

public class ActivateTextBox : MonoBehaviour
{
    // Referencia al GameObject que queremos activar
    public GameObject textBoxGreen;

    // Tiempo en segundos para activar el GameObject
    public float activationDelay = 7f;

    void Start()
    {
        // Desactivamos inicialmente el GameObject si no lo est�
        if (textBoxGreen != null)
        {
            textBoxGreen.SetActive(false);
        }

        // Iniciamos la activaci�n con un retraso
        Invoke("ActivateTextBoxGreen", activationDelay);
    }

    // M�todo para activar el GameObject
    void ActivateTextBoxGreen()
    {
        if (textBoxGreen != null)
        {
            textBoxGreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("El objeto textBoxGreen no est� asignado.");
        }
    }
}
