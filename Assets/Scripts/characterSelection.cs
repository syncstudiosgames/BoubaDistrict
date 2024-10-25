using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterSelection : MonoBehaviour
{
    public GameObject[] characters;
    public int selectedCharacter = 0;

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length ;
        characters[selectedCharacter].SetActive(true);

    }

    public void previousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if(selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
       // logica de implementar el personaje en gameScene
    }
}
