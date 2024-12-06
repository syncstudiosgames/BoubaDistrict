using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    [Header("Posiciones del Podio")]
    public Transform firstPlacePosition;  // Posición del primer lugar
    public Transform secondPlacePosition; // Posición del segundo lugar
    public Transform thirdPlacePosition;  // Posición del tercer lugar

    [Header("Prefabs de Skins")]
    public GameObject kikiPrefab;
    public GameObject kiki2Prefab;
    public GameObject kiki3Prefab;
    public GameObject kikePrefab;
    public GameObject kikoPrefab;
    public GameObject kikaPrefab;
    public GameObject kukiPrefab;



    [Header("Ranking UI")]
    public TMP_Text Nombre1;
    public TMP_Text Nombre2;
    public TMP_Text Nombre3;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private ScrollRect scrollRect;

    private const string serverUrl = "https://highscore-server.glitch.me/api";

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        scrollRect = GetComponentInParent<ScrollRect>();

        StartCoroutine(GetHighscoresFromServer());
    }

    private IEnumerator GetHighscoresFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{serverUrl}/get-scores"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Ranking recibido: " + request.downloadHandler.text);

                HighscoreEntry[] highscores = JsonHelper.FromJson<HighscoreEntry>(request.downloadHandler.text);

                highscoreEntryTransformList = new List<Transform>();

                UpdateTopThreeNamesAndPodium(highscores);

                int mostRecentIndex = -1;
                for (int i = 0; i < highscores.Length; i++)
                {
                    if (highscores[i].id == FinalScore.playerId)
                    {
                        mostRecentIndex = i;
                    }
                }

                for (int i = 0; i < highscores.Length; i++)
                {
                    bool isMostRecent = i == mostRecentIndex;
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

    private void UpdateTopThreeNamesAndPodium(HighscoreEntry[] highscores)
    {
        if (highscores.Length > 0)
        {
            Nombre1.text = highscores[0].name;
            SpawnCharacterOnPodium(firstPlacePosition, highscores[0].characterIndex);
        }
        if (highscores.Length > 1)
        {
            Nombre2.text = highscores[1].name;
            SpawnCharacterOnPodium(secondPlacePosition, highscores[1].characterIndex);
        }
        if (highscores.Length > 2)
        {
            Nombre3.text = highscores[2].name;
            SpawnCharacterOnPodium(thirdPlacePosition, highscores[2].characterIndex);
        }
    }

    private void SpawnCharacterOnPodium(Transform podiumPosition, int characterIndex)
    {
        // Eliminar cualquier objeto hijo previo del podio
        foreach (Transform child in podiumPosition)
        {
            Destroy(child.gameObject);
        }

        GameObject characterPrefab = GetCharacterPrefab(characterIndex);
        if (characterPrefab != null && podiumPosition != null)
        {
            Instantiate(characterPrefab, podiumPosition.position, podiumPosition.rotation, podiumPosition);
        }
    }

    private GameObject GetCharacterPrefab(int characterIndex)
    {
        switch (characterIndex)
        {
            case 0: return kikiPrefab;
            case 1: return kiki2Prefab;
            case 2: return kiki3Prefab;
            case 3: return kikePrefab;
            case 4: return kikoPrefab;
            case 5: return kikaPrefab;
            case 6: return kukiPrefab;

            default:
                Debug.LogError($"Índice de personaje no válido: {characterIndex}");
                return null;
        }
    }

    private void CreateHighscoreEntryTransform(string name, int score, int rank, Transform container, List<Transform> transformList, bool isMostRecent)
    {
        float templateHeight = 80f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        string rankString = "#" + rank;
        entryTransform.Find("PuestoEntrada").GetComponent<Text>().text = rankString;
        entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().text = score.ToString();
        entryTransform.Find("NombreEntrada").GetComponent<Text>().text = name;

        if (isMostRecent)
        {
            entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().color = new Color(1f, 0.956f, 0f); // Amarillo
            entryTransform.Find("NombreEntrada").GetComponent<Text>().color = new Color(1f, 0.956f, 0f);
            entryTransform.Find("PuestoEntrada").GetComponent<Text>().color = new Color(1f, 0.956f, 0f);

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


    private void ScrollToIndex(int index)
    {
        if (scrollRect == null) return;

        RectTransform containerRect = entryContainer.GetComponent<RectTransform>();
        RectTransform entryRect = entryTemplate.GetComponent<RectTransform>();

        float contentHeight = containerRect.rect.height;
        float entryHeight = entryRect.rect.height;
        float targetPosition = Mathf.Clamp01(1f - ((index * entryHeight) / contentHeight));

        scrollRect.verticalNormalizedPosition = targetPosition;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public string name;
        public int score;
        public string id;
        public int characterIndex; 
    }

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
