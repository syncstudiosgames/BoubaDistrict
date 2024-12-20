using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private float delay = 0.2f; // Duraci�n del delay en segundos
    public void OnSettingsButton()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OpenSettings(); // Usar el Singleton para abrir el men� de ajustes
        }
        else
        {
            Debug.LogError("No se encontr� una instancia v�lida de SettingsManager.");
        }
    }
    public void Jugar()
    {
        StartCoroutine(LoadSceneWithDelay("Character Selection"));
    }

    public void Tutorial()
    {
        StartCoroutine(LoadSceneWithDelay("Tutorial"));
    }

    public void Game()
    {
        StartCoroutine(LoadSceneWithDelay("GameScene"));
    }

    public void Logo()
    {
        StartCoroutine(LoadSceneWithDelay("MenuInicio"));
    }

    public void Credits()
    {
        StartCoroutine(LoadSceneWithDelay("Credits"));
    }
    public void Cinematica()
    {
        StartCoroutine(LoadSceneWithDelay("Cinematica"));
    }

    public void CharacterSelection()
    {
        StartCoroutine(LoadSceneWithDelay("Character Selection"));
    }
    public void Salir()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void MainMenu()
    {
        Debug.Log("Inicio");
        StartCoroutine(LoadSceneWithDelay("Inicio"));
    }
    public void RankingUlt()
    {
        Debug.Log("going to ranking");
        StartCoroutine(LoadSceneWithDelay("RankingUlt"));
    }

    public void mapSelection()
    {
        Debug.Log("going to maps");
        StartCoroutine(LoadSceneWithDelay("mapSelection"));
    }

    public void JazzMap()
    {
        Debug.Log("going to jazz map");
        StartCoroutine(LoadSceneWithDelay("Jazz"));
    }

    public void CyberpunkMap()
    {
        Debug.Log("going to cyberpunk map");
        StartCoroutine(LoadSceneWithDelay("Cyberpunk"));
    }


    public void Loading()
    {
        Debug.Log("going to loading");
        StartCoroutine(LoadSceneWithDelay("Loading2"));
    }


    // Corutina para esperar un delay antes de cambiar de escena
    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        SceneManager.LoadScene(sceneName); // Carga la escena
    }
}
