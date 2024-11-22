using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public GameObject settingsPrefab; // Prefab del menú de ajustes
    private GameObject settingsInstance; // Instancia activa del prefab

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Eliminar duplicados
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persistir entre escenas
    }

    public void OpenSettings()
    {
        // Instanciar el prefab si no existe ya una instancia activa
        if (settingsInstance == null)
        {
            settingsInstance = Instantiate(settingsPrefab, transform);
        }
        // Activar el prefab y asegurarse de que esté visible
        settingsInstance.SetActive(true);
    }
}
