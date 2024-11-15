using UnityEngine;

public class PlayerModelLoader : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject defaultCharacter;

    private void Start()
    {
        // Cargar el personaje 
        LoadCharacter();
    }

    private void LoadCharacter()
    {
        if (CharacterSelectionManager.Instance != null)
        {
            GameObject selectedCharacter = CharacterSelectionManager.Instance.GetSelectedCharacter();
            Instantiate(selectedCharacter, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Personaje seleccionado cargado.");
        }
        else
        {
            Instantiate(defaultCharacter, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Personaje por defecto cargado.");
        }
    }
}
