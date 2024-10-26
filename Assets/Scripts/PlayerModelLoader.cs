using UnityEngine;

public class PlayerModelLoader : MonoBehaviour
{
    public Transform spawnPoint; 

    private void Start()
    {
        if (CharacterSelectionManager.Instance != null && CharacterSelectionManager.Instance.selectedCharacter != null)
        {
            Instantiate(CharacterSelectionManager.Instance.selectedCharacter, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("No se seleccionó ningún personaje");
        }
    }
}
