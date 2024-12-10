using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight; // Asigna tu Directional Light
    public List<Light> pointLights; // Asigna tus Point Lights aquí
    public float dayDuration = 30f; // Duración de cada ciclo (30 segundos)
    public float transitionDuration = 5f; // Duración de la transición (5 segundos)
    public Color dayColor = Color.white; // Color de la luz para el día
    public Color nightColor = new Color(0.2f, 0.2f, 0.5f); // Color de la luz para la noche (tono azulado)

    private bool isDay = true;
    private bool pointLightsActivated = false;

    void Start()
    {
        // Configuración inicial: Día
        SetDayImmediate();
        StartCoroutine(CycleDayNight());
    }

    IEnumerator CycleDayNight()
    {
        while (true)
        {
            yield return new WaitForSeconds(dayDuration);

            if (isDay)
            {
                StartCoroutine(FadeToNight());
            }
            else
            {
                StartCoroutine(FadeToDay());
            }
        }
    }

    IEnumerator FadeToNight()
    {
        isDay = false;
        pointLightsActivated = false; // Asegurar que las luces se activan en el momento adecuado

        // Interpolar la intensidad y el color de la Directional Light hacia "noche"
        float startIntensity = directionalLight.intensity;
        float endIntensity = 0.1f; // Intensidad más oscura para la noche
        Color startColor = directionalLight.color;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / transitionDuration);
            directionalLight.color = Color.Lerp(startColor, nightColor, elapsed / transitionDuration);

            // Encender las Point Lights cuando la intensidad esté por debajo de 0.3
            if (!pointLightsActivated && directionalLight.intensity <= 0.3f)
            {
                foreach (Light pointLight in pointLights)
                {
                    pointLight.enabled = true;
                }
                pointLightsActivated = true; // Evitar que se activen múltiples veces
            }

            yield return null;
        }

        directionalLight.intensity = endIntensity; // Asegurar que alcanza la intensidad final
        directionalLight.color = nightColor; // Asegurar que el color es el final
    }

    IEnumerator FadeToDay()
    {
        isDay = true;

        // Interpolar la intensidad y el color de la Directional Light hacia "día"
        float startIntensity = directionalLight.intensity;
        float endIntensity = 1f; // Intensidad para el día
        Color startColor = directionalLight.color;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / transitionDuration);
            directionalLight.color = Color.Lerp(startColor, dayColor, elapsed / transitionDuration);
            yield return null;
        }

        directionalLight.intensity = endIntensity; // Asegurar que alcanza la intensidad final
        directionalLight.color = dayColor; // Asegurar que el color es el final

        // Apagar las Point Lights al final de la transición
        foreach (Light pointLight in pointLights)
        {
            pointLight.enabled = false;
        }
    }

    void SetDayImmediate()
    {
        isDay = true;
        if (directionalLight != null)
        {
            directionalLight.intensity = 1f; // Intensidad de día inicial
            directionalLight.color = dayColor; // Color de día inicial
        }

        foreach (Light pointLight in pointLights)
        {
            pointLight.enabled = false; // Apaga las Point Lights
        }
    }
}