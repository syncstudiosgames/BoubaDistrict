using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private float delay = 0.2f; // Duración del delay en segundos
    public void OnSettingsButton()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.OpenSettings(); // Usar el Singleton para abrir el menú de ajustes
        }
        else
        {
            Debug.LogError("No se encontró una instancia válida de SettingsManager.");
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
    // Corutina para esperar un delay antes de cambiar de escena
    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        SceneManager.LoadScene(sceneName); // Carga la escena
    }
}
