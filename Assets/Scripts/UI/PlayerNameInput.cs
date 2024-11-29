using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI displayText;
    private const int maxCharacters = 5;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Todos los datos de PlayerPrefs han sido eliminados.");

        // Asegurarse de que el campo de entrada esté vacío al inicio
        nameInputField.text = "";
        nameInputField.onValueChanged.AddListener(OnNameInputChanged);
    }
    private void OnNameInputChanged(string currentText)
    {
        if (currentText.Length > maxCharacters)
        {
            nameInputField.text = currentText.Substring(0, maxCharacters);
        }
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
            displayText.text = "Nice to see you, " + playerName + "! Let's play!";
        }
    }

}
