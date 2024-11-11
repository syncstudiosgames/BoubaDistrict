using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI displayText;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los datos de PlayerPrefs han sido eliminados.");

        // Asegurarse de que el campo de entrada esté vacío al inicio
        nameInputField.text = "";
    }

    public void OnNameEntered()
    {
        // Guardar el nombre ingresado en PlayerPrefs para acceder a él en otras escenas
        string playerName = nameInputField.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        // Mostrar un mensaje de confirmación
        if (displayText != null)
        {
            displayText.text = "Bienvenido, " + playerName + "! Nombre guardado.";
        }
    }

}
