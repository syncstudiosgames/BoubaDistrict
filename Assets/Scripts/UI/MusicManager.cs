using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Cancelar suscripci�n al evento de cambio de escena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // M�todo llamado cuando se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verificar si la m�sica debe detenerse en ciertas escenas
        if (scene.name == "GameScene" || scene.name == "Jazz" || scene.name == "Cyberpunk" || scene.name == "GameOver" || scene.name == "Tutorial")
        {
            StopMusic();
        }
        else
        {
            // Reproducir la m�sica en otras escenas (por ejemplo, el men� principal)
            PlayMusic();
        }
    }

    // M�todo para detener la m�sica
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // M�todo para reproducir la m�sica
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
