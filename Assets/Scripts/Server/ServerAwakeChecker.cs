using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerAwakeChecker : MonoBehaviour
{
    private string serverUrl = "https://highscore-server.glitch.me/"; // servidor
    private float minWaitTime = 4f; // Tiempo m�nimo de espera 
    public Slider loadingBar; 

    private void Start()
    {
        if (loadingBar != null)
        {
            loadingBar.value = 0f;
        }

        StartCoroutine(WaitForServerAndLoadGameScene());
    }

    private IEnumerator WaitForServerAndLoadGameScene()
    {
        bool serverIsAwake = false;
        float startTime = Time.time;

        Debug.Log("Verificando si el servidor est� despierto...");

        while (!serverIsAwake)
        {
            UnityWebRequest request = UnityWebRequest.Get(serverUrl);
            request.timeout = 5;

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("El servidor est� despierto.");
                serverIsAwake = true;
            }
            else
            {
                Debug.LogWarning("El servidor no est� disponible. Reintentando en 5 segundos...");
                yield return new WaitForSeconds(5);
            }
        }

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minWaitTime)
        {
            while (elapsedTime < minWaitTime)
            {
                elapsedTime = Time.time - startTime;
                if (loadingBar != null)
                {
                    loadingBar.value = Mathf.Clamp01(elapsedTime / minWaitTime);
                }
                yield return null;
            }
        }

        if (loadingBar != null)
        {
            loadingBar.value = 1f;
        }

        SceneManager.LoadScene("MenuInicio");
    }
}
