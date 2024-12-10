using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightController : MonoBehaviour
{
    public Transform playerPosition; // Posición del jugador
    public List<Light> spotlights; 
    public float flickerIntervalMin = 5f; // Tiempo min parpadeo
    public float flickerIntervalMax = 15f; // Tiempo max
    public float flickerDuration = 0.5f; // Duración del parpadeo
    public float wobbleAmount = 5f; // Amplitud 
    public float wobbleSpeed = 2f; // Velocidad del movimiento

    void Start()
    {
        StartCoroutine(RandomFlicker());
    }

    void Update()
    {
        foreach (Light spotlight in spotlights)
        {
            if (spotlight != null)
            {
                // Calcular el meneo del foco aprox
                float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;

                // Apuntar al jugador y que se mueva en el eje X
                Vector3 directionToPlayer = playerPosition.position - spotlight.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                spotlight.transform.rotation = Quaternion.Euler(
                    targetRotation.eulerAngles.x + wobble, 
                    targetRotation.eulerAngles.y,
                    targetRotation.eulerAngles.z
                );
            }
        }
    }

    IEnumerator RandomFlicker()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio entre parpadeos
            float waitTime = Random.Range(flickerIntervalMin, flickerIntervalMax);
            yield return new WaitForSeconds(waitTime);

            // Parpadeo: Apagar y encender las luces
            foreach (Light spotlight in spotlights)
            {
                if (spotlight != null) spotlight.enabled = false;
            }

            yield return new WaitForSeconds(flickerDuration);

            foreach (Light spotlight in spotlights)
            {
                if (spotlight != null) spotlight.enabled = true;
            }
        }
    }
}