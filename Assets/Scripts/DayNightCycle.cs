using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight; 
    public List<Light> pointLights; 
    public float dayDuration; 
    public float transitionDuration; 
    public Color dayColor = Color.white; 
    public Color nightColor = new Color(0.2f, 0.2f, 0.5f); 
    public Vector3 dayRotation = new Vector3(50f, 0f, 0f);
    public Vector3 nightRotation = new Vector3(-30f, 0f, 0f);

    private bool isDay = true;
    private bool pointLightsActivated = false;

    void Start()
    {
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
        pointLightsActivated = false; 

        float startIntensity = directionalLight.intensity;
        float endIntensity = 0.1f; 
        Color startColor = directionalLight.color;
        Quaternion startRotation = Quaternion.Euler(dayRotation);
        Quaternion endRotation = Quaternion.Euler(nightRotation);
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / transitionDuration);
            directionalLight.color = Color.Lerp(startColor, nightColor, elapsed / transitionDuration);
            directionalLight.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / transitionDuration);

            if (!pointLightsActivated && directionalLight.intensity <= 0.3f)
            {
                foreach (Light pointLight in pointLights)
                {
                    pointLight.enabled = true;
                }
                pointLightsActivated = true; 
            }

            yield return null;
        }

        directionalLight.intensity = endIntensity; 
        directionalLight.color = nightColor; 
        directionalLight.transform.rotation = endRotation; 
    }

    IEnumerator FadeToDay()
    {
        isDay = true;

        float startIntensity = directionalLight.intensity;
        float endIntensity = 1f; 
        Color startColor = directionalLight.color;
        Quaternion startRotation = Quaternion.Euler(nightRotation);
        Quaternion endRotation = Quaternion.Euler(dayRotation);
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            directionalLight.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / transitionDuration);
            directionalLight.color = Color.Lerp(startColor, dayColor, elapsed / transitionDuration);
            directionalLight.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / transitionDuration);
            yield return null;
        }

        directionalLight.intensity = endIntensity; 
        directionalLight.color = dayColor; 
        directionalLight.transform.rotation = endRotation; 

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
            directionalLight.intensity = 1f; 
            directionalLight.color = dayColor;
            directionalLight.transform.rotation = Quaternion.Euler(dayRotation); 
        }

        foreach (Light pointLight in pointLights)
        {
            pointLight.enabled = false; 
        }
    }
}
