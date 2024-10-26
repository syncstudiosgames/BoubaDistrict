using UnityEngine;

public class PlayerModelLoader : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject defaultCharacter;


    private void Start()
    {

        if (CharacterSelectionManager.Instance != null)
        {
            GameObject selectedCharacter = CharacterSelectionManager.Instance.GetSelectedCharacter();
            Instantiate(selectedCharacter, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Instantiate(defaultCharacter, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
