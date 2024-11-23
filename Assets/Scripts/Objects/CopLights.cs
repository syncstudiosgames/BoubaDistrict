using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopLights : MonoBehaviour
{
    // Velocidad de rotaci�n de la luz
    public float rotationSpeed = 200f;

    // Control del parpadeo
    public bool enableBlinking = true; // Activar o desactivar parpadeo
    public float blinkInterval = 2f; // Tiempo entre parpadeos (ajustado para menos frecuencia)
    private float blinkTimer = 0f; // Temporizador interno

    // Intensidad din�mica
    public bool enableDynamicIntensity = true; // Activar o desactivar variaciones en la intensidad
    private Light lightComponent;
    public float minIntensity = 2f; // Intensidad m�nima
    public float maxIntensity = 8f; // Intensidad m�xima

    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            Debug.LogWarning("No se encontr� un componente Light en " + gameObject.name);
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

        // Variaci�n din�mica de intensidad
        if (enableDynamicIntensity && lightComponent != null)
        {
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time, 1f));
            lightComponent.intensity = intensity;
        }
    }
}
