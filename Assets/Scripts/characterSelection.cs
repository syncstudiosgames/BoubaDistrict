using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject charactersParent;
    public GameObject[] characters; // Array de personajes disponibles
    public int selectedCharacter = 0; // �ndice del personaje seleccionado

    private void Start()
    {
        foreach (Transform child in charactersParent.transform)
        {
            child.gameObject.SetActive(false);
        }
        charactersParent.transform.GetChild(selectedCharacter).gameObject.SetActive(true);
    }

    public void NextCharacter()
    {
        charactersParent.transform.GetChild(selectedCharacter).gameObject.SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        charactersParent.transform.GetChild(selectedCharacter).gameObject.SetActive(true);
    }

    public void PreviousCharacter()
    {
        charactersParent.transform.GetChild(selectedCharacter).gameObject.SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        charactersParent.transform.GetChild(selectedCharacter).gameObject.SetActive(true);
    }

    public void StartGame()
    {
        // Seleccionar el personaje a trav�s del CharacterSelectionManager
        CharacterSelectionManager.Instance.SelectCharacter(characters[selectedCharacter]);

        // Cargar la escena del juego
        SceneManager.LoadScene("GameScene");
    }
}