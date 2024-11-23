using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopLights : MonoBehaviour
{
    // Velocidad de rotación de la luz
    public float rotationSpeed = 200f;

    // Control del parpadeo
    public bool enableBlinking = true; // Activar o desactivar parpadeo
    public float blinkInterval = 2f; // Tiempo entre parpadeos (ajustado para menos frecuencia)
    private float blinkTimer = 0f; // Temporizador interno

    // Intensidad dinámica
    public bool enableDynamicIntensity = true; // Activar o desactivar variaciones en la intensidad
    private Light lightComponent;
    public float minIntensity = 2f; // Intensidad mínima
    public float maxIntensity = 8f; // Intensidad máxima

    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            Debug.LogWarning("No se encontró un componente Light en " + gameObject.name);
        }
    }

    void Update()
    {
        // Rotaion
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotationSpeed, 0f));

        // Parpadeo controlado
        if (enableBlinking && lightComponent != null)
        {
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= blinkInterval) // Controlar la frecuencia del parpadeo
            {
                lightComponent.enabled = !lightComponent.enabled; // Alternar encendido/apagado
                blinkTimer = 0f;
            }
        }

        // Variación dinámica de intensidad
        if (enableDynamicIntensity && lightComponent != null)
        {
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time, 1f));
            lightComponent.intensity = intensity;
        }
    }
}
