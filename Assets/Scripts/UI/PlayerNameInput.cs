using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TextMeshProUGUI displayText;
    public Button acceptButton;
    private const int maxCharacters = 5;

    private const string PlayerNameKey = "PlayerName";

    private void Start()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            PlayerPrefs.DeleteKey(PlayerNameKey);
            Debug.Log($"PlayerPref '{PlayerNameKey}' ha sido eliminado.");
        }

        nameInputField.text = "";
        nameInputField.onValueChanged.AddListener(OnNameInputChanged);

        acceptButton.interactable = false;
    }

    private void OnNameInputChanged(string currentText)
    {
        if (currentText.Length > maxCharacters)
        {
            nameInputField.text = currentText.Substring(0, maxCharacters);
        }

        acceptButton.interactable = !string.IsNullOrEmpty(currentText);
    }

    public void OnNameEntered()
    {
        string playerName = nameInputField.text;
        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.Save();

        if (displayText != null)
        {
            displayText.text = "Nice to see you, " + playerName + "! Let's play!";
        }
    }
}