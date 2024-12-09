using UnityEngine;

public class MenuCharacterDisplay : MonoBehaviour
{
    public Transform characterSpawnPoint; 
    public GameObject[] characterPrefabs; 
    public GameObject defaultCharacterPrefab; 

    private void Start()
    {
        if (CharacterSelectionManager.Instance != null)
        {
            int selectedIndex = CharacterSelectionManager.Instance.GetSelectedCharacterIndex();

            if (selectedIndex >= 0 && selectedIndex < characterPrefabs.Length)
            {
                GameObject prefabToSpawn = characterPrefabs[selectedIndex];
                Instantiate(prefabToSpawn, characterSpawnPoint.position, Quaternion.identity, characterSpawnPoint);
            }
            else
            {
                Debug.LogWarning("Selected index is out of bounds, instantiating default character.");
                Instantiate(defaultCharacterPrefab, characterSpawnPoint.position, Quaternion.identity, characterSpawnPoint);
            }
        }
        else
        {
            Debug.LogWarning("CharacterSelectionManager instance is null, instantiating default character.");
            Instantiate(defaultCharacterPrefab, characterSpawnPoint.position, Quaternion.identity, characterSpawnPoint);
        }
    }
}