using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    [SerializeField] private GameObject[] characters; // Array de personajes disponibles
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

    public void SetCharacters(GameObject[] characterArray)
    {
        characters = characterArray;
    }

    public GameObject[] GetCharacters()
    {
        return characters;
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
