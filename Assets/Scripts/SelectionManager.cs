using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance; 
    public GameObject selectedCharacter;

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

    // M�todo para seleccionar el personaje
    public void SelectCharacter(GameObject character)
    {
        selectedCharacter = character;
    }
}
