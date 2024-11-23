using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//using TMPro.EditorUtilities;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private ScrollRect scrollRect;

    public TMP_Text Nombre1;
    public TMP_Text Nombre2;
    public TMP_Text Nombre3;

    // Servidor público en Glitch
    private const string serverUrl = "https://highscore-server.glitch.me/api";

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        // Obtener el componente ScrollRect del ScrollView
        scrollRect = GetComponentInParent<ScrollRect>();

        // Obtener el ranking desde el servidor al cargar la escena
        StartCoroutine(GetHighscoresFromServer());
    }

    private void CreateHighscoreEntryTransform(string name, int score, int rank, Transform container, List<Transform> transformList, bool isMostRecent)
    {
        float templateHeight = 80f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string rankString = "#" + rank ;
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
            entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().color = Color.white;
            entryTransform.Find("NombreEntrada").GetComponent<Text>().color = Color.white;
            entryTransform.Find("PuestoEntrada").GetComponent<Text>().color = Color.white;
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

                // Actualizar los nombres de los tres primeros jugadores
                UpdateTopThreeNames(highscores);

                // Identificar la puntuación más reciente por alguna lógica (por ejemplo, el último ID)
                int mostRecentIndex = -1;
                for (int i = 0; i < highscores.Length; i++)
                {
                    if (highscores[i].id == FinalScore.playerId)
                    {
                        mostRecentIndex = i;
                    }
                }

                // Crear las entradas del ranking
                for (int i = 0; i < highscores.Length; i++)
                {
                    bool isMostRecent = i == mostRecentIndex; // Solo resaltar la más reciente
                    CreateHighscoreEntryTransform(highscores[i].name, highscores[i].score, i + 1, entryContainer, highscoreEntryTransformList, isMostRecent);
                }

                if (mostRecentIndex != -1)
                {
                    ScrollToIndex(mostRecentIndex);
                }
            }
            else
            {
                Debug.LogError($"Error al obtener el ranking: {request.error}");
            }
        }
    }
    private void ScrollToIndex(int index)
    {
        // Asegurarse de que el ScrollRect esté configurado
        if (scrollRect == null) return;

        // Obtener el tamaño del contenedor y el template
        RectTransform containerRect = entryContainer.GetComponent<RectTransform>();
        RectTransform entryRect = entryTemplate.GetComponent<RectTransform>();

        // Calcular la altura total y la posición objetivo
        float contentHeight = containerRect.rect.height;
        float entryHeight = entryRect.rect.height;
        float targetPosition = Mathf.Clamp01(1f - ((index * entryHeight) / contentHeight));

        // Ajustar la posición del ScrollRect
        scrollRect.verticalNormalizedPosition = targetPosition;
    }
    private void UpdateTopThreeNames(HighscoreEntry[] highscores)
    {
        if (highscores.Length > 0)
            Nombre1.text = highscores[0].name;
        if (highscores.Length > 1)
            Nombre2.text = highscores[1].name;
        if (highscores.Length > 2)
            Nombre3.text = highscores[2].name;
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
