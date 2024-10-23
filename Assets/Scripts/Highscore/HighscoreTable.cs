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
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
            highscoreEntryList = highscores.scoreList;
        }
        else
        {
            Debug.LogError("El archivo JSON no se encontró en " + filePath);
            highscoreEntryList = new List<HighscoreEntry>(); // Evitar que esté vacío si no se encuentra el archivo
        }

        // Ordenar la lista de puntuaciones
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = 0; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score < highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }

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

        string name = highscoreEntry.name;
        entryTransform.Find("NombreEntrada").GetComponent<Text>().text = name;

        transformList.Add(entryTemplate);
    }

    private class Highscores
    {
        public List<HighscoreEntry> scoreList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
