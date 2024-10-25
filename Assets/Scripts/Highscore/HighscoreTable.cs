using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Cargar datos desde el archivo JSON
        string filePath = Application.dataPath + "/Scripts/Highscore/ScoreData.json";
        highscoreEntryList = new List<HighscoreEntry>();

        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

            // Convertir cada puntaje en `Scores` a un `HighscoreEntry`
            foreach (int score in highscores.Scores)
            {
                highscoreEntryList.Add(new HighscoreEntry { score = score });
            }
        }
        else
        {
            Debug.LogError("El archivo JSON no se encontró en " + filePath);
        }

        // Ordenar la lista de puntuaciones
        highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString = rank + "º";
        entryTransform.Find("PuestoEntrada").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().text = score.ToString();

        string name = "AAA";
        entryTransform.Find("NombreEntrada").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
    }

    [System.Serializable]
    private class Highscores
    {
        public List<int> Scores;  // Cambiado para coincidir con el JSON
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        //public string name;
    }
}
