using UnityEngine;

public class PauseButton : MonoBehaviour
{
    // Referencias a los GameObjects
    public GameObject pauseMenu;
    public GameObject fade;

    // M�todo para manejar el clic en el bot�n
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
            Debug.LogWarning("Aseg�rate de asignar el PauseMenu y el Fade en el Inspector.");
        }
    }
}
