using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    // Servidor público en Glitch
    private const string serverUrl = "https://highscore-server.glitch.me/api";

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Obtener el ranking desde el servidor al cargar la escena
        StartCoroutine(GetHighscoresFromServer());
    }

    private void CreateHighscoreEntryTransform(string name, int score, int rank, Transform container, List<Transform> transformList, bool isMostRecent)
    {
        float templateHeight = 40f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string rankString = rank + "º";
        entryTransform.Find("PuestoEntrada").GetComponent<Text>().text = rankString;
        entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().text = score.ToString();
        entryTransform.Find("NombreEntrada").GetComponent<Text>().text = name;

        // Verifica si el id del jugador coincide con el id del servidor para resaltar su entrada**
        if (isMostRecent)
        {
            // Resaltar con color amarillo
            entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().color = new Color(1f, 0.956f, 0f); // Amarillo
            entryTransform.Find("NombreEntrada").GetComponent<Text>().color = new Color(1f, 0.956f, 0f);
            entryTransform.Find("PuestoEntrada").GetComponent<Text>().color = new Color(1f, 0.956f, 0f);

            // Añadir borde negro al texto
            Outline outline1 = entryTransform.Find("PuntuaciónEntrada").gameObject.AddComponent<Outline>();
            outline1.effectColor = Color.black;
            outline1.effectDistance = new Vector2(2, -2);

            Outline outline2 = entryTransform.Find("NombreEntrada").gameObject.AddComponent<Outline>();
            outline2.effectColor = Color.black;
            outline2.effectDistance = new Vector2(2, -2);

            Outline outline3 = entryTransform.Find("PuestoEntrada").gameObject.AddComponent<Outline>();
            outline3.effectColor = Color.black;
            outline3.effectDistance = new Vector2(2, -2);
        }
        else
        {
            entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().color = Color.black;
            entryTransform.Find("NombreEntrada").GetComponent<Text>().color = Color.black;
            entryTransform.Find("PuestoEntrada").GetComponent<Text>().color = Color.black;
        }

        transformList.Add(entryTransform);
    }

    // Obtener el ranking desde el servidor
    private IEnumerator GetHighscoresFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{serverUrl}/get-scores"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Ranking recibido: " + request.downloadHandler.text);

                // Convertir el JSON a una lista de objetos HighscoreEntry
                HighscoreEntry[] highscores = JsonHelper.FromJson<HighscoreEntry>(request.downloadHandler.text);

                // Crear la visualización de cada entrada del ranking
                highscoreEntryTransformList = new List<Transform>();
                for (int i = 0; i < highscores.Length; i++)
                {
                    // Verificar si el ID del jugador coincide con el del ranking
                    bool isMostRecent = highscores[i].id == FinalScore.playerId; 

                    CreateHighscoreEntryTransform(highscores[i].name, highscores[i].score, i + 1, entryContainer, highscoreEntryTransformList, isMostRecent);
                }
            }
            else
            {
                Debug.LogError($"Error al obtener el ranking: {request.error}");
            }
        }
    }


    [System.Serializable]
    private class HighscoreEntry
    {
        public string name;
        public int score;
        public string id;  
    }

    // para deserializar arrays de JSON
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
