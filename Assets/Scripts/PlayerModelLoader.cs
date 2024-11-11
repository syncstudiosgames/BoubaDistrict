using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelLoader : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject defaultCharacter;

    private string serverUrl = "https://highscore-server.glitch.me/"; // URL del servidor

    private void Start()
    {
        // Cargar el personaje 
        LoadCharacter();

        // Iniciar la verificación del servidor en segundo plano
        StartCoroutine(EnsureServerAwake());
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

    private IEnumerator EnsureServerAwake()
    {
        bool serverIsAwake = false;

        Debug.Log("Verificando si el servidor está despierto...");

        while (!serverIsAwake)
        {
            UnityWebRequest request = UnityWebRequest.Get(serverUrl);
            request.timeout = 5;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("El servidor está despierto.");
                serverIsAwake = true;
            }
            else
            {
                Debug.LogWarning("El servidor no está disponible. Reintentando en 5 segundos...");
                yield return new WaitForSeconds(5);
            }
        }

        Debug.Log("El servidor ya está despierto. Puedes enviar datos ahora.");
    }
}
