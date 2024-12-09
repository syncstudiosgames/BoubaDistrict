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
        // Cancelar suscripción al evento de cambio de escena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método llamado cuando se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verificar si la escena es "GameScene"
        if (scene.name == "GameScene")
        {
            StopMusic();
        }
        if (scene.name == "Jazz")
        {
            StopMusic();
        }
        if (scene.name == "Cyberpunk")
        {
            StopMusic();
        }
        if (scene.name == "RankingUlt")
        {
            StopMusic();
        }
    }

    // Método para detener la música
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
