using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    [SerializeField] private GameObject[] characters; 
    private GameObject selectedCharacter;
    private int selectedCharacterIndex = 0; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
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

    public void SelectCharacter(GameObject character, int index)
    {
        selectedCharacter = character;
        selectedCharacterIndex = index; 
    }

    public GameObject GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public int GetSelectedCharacterIndex()
    {
        return selectedCharacterIndex; 
    }
}