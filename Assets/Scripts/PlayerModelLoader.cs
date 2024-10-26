using UnityEngine;

public class PlayerModelLoader : MonoBehaviour
{
    public Transform spawnPoint; 

    private void Start()
    {
        GameObject selectedCharacter = CharacterSelectionManager.Instance.GetSelectedCharacter();

        if (selectedCharacter != null)
        {
            Instantiate(selectedCharacter, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("No se seleccionó ningún personaje");
        }
    }
}
