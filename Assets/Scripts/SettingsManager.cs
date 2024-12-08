using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public GameObject settingsPrefab; // Prefab del menú de ajustes
    private GameObject settingsInstance; // Instancia activa del prefab
    public GameObject fade; // objeto fade


    private void Awake()
    {
        // Implementación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Eliminar duplicados
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject); // Persistir entre escenas
    }

    public void OpenSettings()
    {
        if (settingsInstance == null)
        {
            // Crear instancia del prefab si no existe
            settingsInstance = Instantiate(settingsPrefab, transform);
            fade.SetActive(true);
        }
        else
        {
            // Si ya existe, eliminar la instancia
            Destroy(settingsInstance);
            settingsInstance = null;
            fade.SetActive(false);

        }
    }
}
