using UnityEngine;
using UnityEngine.SceneManagement; // Importante para manejar las escenas

public class PauseButton : MonoBehaviour
{
    // Referencias a los GameObjects
    public GameObject pauseMenu;
    public GameObject fade;

    private bool isPaused = false; // Variable para controlar el estado de pausa

    // Método para manejar el clic en el botón de pausa
    public void TogglePause()
    {
        if (pauseMenu != null && fade != null)
        {
            // Cambia el estado de pausa
            isPaused = !isPaused;

            // Activa o desactiva los GameObjects
            pauseMenu.SetActive(isPaused);
            fade.SetActive(isPaused);

            // Pausa o reanuda el juego
            Time.timeScale = isPaused ? 0 : 1;
        }
        else
        {
            Debug.LogWarning("Asegúrate de asignar el PauseMenu y el Fade en el Inspector.");
        }
    }

    // Método para cargar una nueva escena desde el menú de pausa
    public void MenuInicio()
    {
        // Asegura que el tiempo vuelve a su escala normal antes de cambiar de escena
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuInicio");
    }

    public void RankingUlt()
    {
        // Asegura que el tiempo vuelve a su escala normal antes de cambiar de escena
        Time.timeScale = 1;
        SceneManager.LoadScene("RankingUlt");
    }

}
