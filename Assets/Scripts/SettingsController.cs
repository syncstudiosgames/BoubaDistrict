using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private Slider musicSlider;
    private Slider effectsSlider;
    private Button acceptButton;

    private const string MusicVolumeKey = "MusicVolume";
    private const string EffectsVolumeKey = "EffectsVolume";

    private void Awake()
    {
        musicSlider = FindChildByName<Slider>("SliderMusic");
        effectsSlider = FindChildByName<Slider>("SliderEffects");
        acceptButton = FindChildByName<Button>("AcceptButton");

        if (musicSlider == null || effectsSlider == null || acceptButton == null)
        {
            Debug.LogError("No se pudieron encontrar los componentes necesarios. Revisa la jerarquía del prefab.");
            Debug.LogError("Componentes encontrados: " +
                $"\nAcceptButton: {(acceptButton != null ? "Sí" : "No")}" +
                $"\nEffectsSlider: {(effectsSlider != null ? "Sí" : "No")}" +
                $"\nMusicSlider: {(musicSlider != null ? "Sí" : "No")}");
            return;
        }

        acceptButton.onClick.AddListener(SaveSettings);

        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);

        LoadSettings();
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
        PlayerPrefs.SetFloat(MusicVolumeKey, musicSlider.value);
        PlayerPrefs.SetFloat(EffectsVolumeKey, effectsSlider.value);

        ApplySettings();

        gameObject.SetActive(false);
    }

    public void LoadSettings()
    {
        // Cargar valores de PlayerPrefs
        musicSlider.value = PlayerPrefs.HasKey(MusicVolumeKey) ? PlayerPrefs.GetFloat(MusicVolumeKey) : 1.0f;
        effectsSlider.value = PlayerPrefs.HasKey(EffectsVolumeKey) ? PlayerPrefs.GetFloat(EffectsVolumeKey) : 1.0f;
    }

    private void ApplySettings()
    {
        AudioListener.volume = musicSlider.value;
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioListener.volume = value;
    }

    private void OnEffectsSliderChanged(float value)
    {
        Debug.Log($"Volumen de efectos ajustado a: {value}");
    }
}
