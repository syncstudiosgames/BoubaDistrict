using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private float delay = 0.2f; // Duración del delay en segundos

    public void Jugar()
    {
        StartCoroutine(LoadSceneWithDelay("Character Selection"));
    }

    public void Logo()
    {
        StartCoroutine(LoadSceneWithDelay("MenuInicio"));
    }

    public void Credits()
    {
        StartCoroutine(LoadSceneWithDelay("Credits"));
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

    // Corutina para esperar un delay antes de cambiar de escena
    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo especificado
        SceneManager.LoadScene(sceneName); // Carga la escena
    }
}
