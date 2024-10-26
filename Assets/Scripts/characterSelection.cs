using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characters; // Array de personajes disponibles
    public int selectedCharacter = 0; // �ndice del personaje seleccionado

    private void Start()
    {
        // Asegurarse de que solo el personaje seleccionado est� activo al inicio
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        // Seleccionar el personaje a trav�s del CharacterSelectionManager
        CharacterSelectionManager.Instance.SelectCharacter(characters[selectedCharacter]);

        // Cargar la escena del juego
        SceneManager.LoadScene("GameScene");
    }
}
