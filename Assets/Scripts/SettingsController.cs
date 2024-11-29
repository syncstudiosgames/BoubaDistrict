using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private Slider musicSlider;
    private Slider effectsSlider;
    private Button acceptButton;

    private const string MusicVolumeKey = "MusicVolume";
    private const string EffectsVolumeKey = "EffectsVolume";

    private static SettingsController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados si ya existe un controlador
        }

        musicSlider = FindChildByName<Slider>("SliderMusic");
        effectsSlider = FindChildByName<Slider>("SliderEffects");
        acceptButton = FindChildByName<Button>("AcceptButton");

        if (musicSlider == null || effectsSlider == null || acceptButton == null)
        {
            Debug.LogError("No se pudieron encontrar los componentes necesarios. Revisa la jerarquía del prefab.");
            return;
        }

        acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);

        LoadSettings();
    }


    private void OnEnable()
    {
        LoadSettings();
    }

    private void OnDisable()
    {
        SaveSettings();
    }

    private T FindChildByName<T>(string childName) where T : Component
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.name == childName)
            {
                return child.GetComponent<T>();
            }
        }

        Debug.LogError($"No se encontró un hijo con el nombre: {childName}");
        return null;
    }

    public void SaveSettings()
    {
        Debug.Log($"Guardando ajustes: Música = {musicSlider.value}, Efectos = {effectsSlider.value}");
        PlayerPrefs.SetFloat(MusicVolumeKey, musicSlider.value);
        PlayerPrefs.SetFloat(EffectsVolumeKey, effectsSlider.value);
        PlayerPrefs.Save();

        ApplySettings();
    }

    public void LoadSettings()
    {
        float defaultMusic = 1.0f;
        float defaultEffects = 1.0f;

        float musicVolume = PlayerPrefs.HasKey(MusicVolumeKey) ? PlayerPrefs.GetFloat(MusicVolumeKey) : defaultMusic;
        float effectsVolume = PlayerPrefs.HasKey(EffectsVolumeKey) ? PlayerPrefs.GetFloat(EffectsVolumeKey) : defaultEffects;

        Debug.Log($"Cargando ajustes: Música = {musicVolume}, Efectos = {effectsVolume}");

        musicSlider.value = musicVolume;
        effectsSlider.value = effectsVolume;

        ApplySettings();
    }


    private void ApplySettings()
    {
        AudioListener.volume = musicSlider.value;

        Debug.Log($"Aplicados ajustes: Música = {musicSlider.value}, Efectos = {effectsSlider.value}");
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioListener.volume = value;
    }

    private void OnEffectsSliderChanged(float value)
    {
        Debug.Log($"Volumen de efectos ajustado a: {value}");
    }

    private void OnAcceptButtonClicked()
    {
        SaveSettings();
        gameObject.SetActive(false); 
    }
}
