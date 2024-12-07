using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // Referencias a los GameObjects
    public GameObject pauseMenu;
    public GameObject fade;

    // Método para manejar el clic en el botón
    public void TogglePause()
    {
        if (pauseMenu != null && fade != null)
        {
            // Cambia el estado de pausa
            bool isPaused = pauseMenu.activeSelf;

            // Activa o desactiva los GameObjects
            pauseMenu.SetActive(!isPaused);
            fade.SetActive(!isPaused);

            // Pausa o reanuda el juego
            Time.timeScale = isPaused ? 1 : 0;
        }
        else
        {
            Debug.LogWarning("Asegúrate de asignar el PauseMenu y el Fade en el Inspector.");
        }
    }
}
