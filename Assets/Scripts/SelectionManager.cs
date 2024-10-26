using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;
    private GameObject selectedCharacter;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectCharacter(GameObject character)
    {
        selectedCharacter = character;
    }

    public GameObject GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}
