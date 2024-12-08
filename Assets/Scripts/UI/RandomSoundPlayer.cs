using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // Lista de sonidos
    private AudioSource audioSource;

    void Start()
    {
        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un componente AudioSource en este GameObject.");
        }
    }

    public void PlayRandomSound()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No hay clips de audio asignados.");
            return;
        }

        // Seleccionar un clip aleatorio
        int randomIndex = Random.Range(0, audioClips.Length);
        AudioClip selectedClip = audioClips[randomIndex];

        // Reproducir el clip
        audioSource.PlayOneShot(selectedClip);
    }
}
