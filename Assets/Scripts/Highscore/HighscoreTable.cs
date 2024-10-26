using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

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

        // Marcar el último elemento añadido como el "más reciente"
        HighscoreEntry mostRecentEntry = highscoreEntryList[highscoreEntryList.Count - 1];

        // Ordenar la lista de puntuaciones
        highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            // Enviar un indicador si el elemento es el "más reciente"
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList, highscoreEntry == mostRecentEntry);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList, bool isMostRecent)
    {
        float templateHeight = 40f;

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

        // Cambiar el color del texto del puntaje
        if (isMostRecent)
        {
            entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("NombreEntrada").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("PuestoEntrada").GetComponent<Text>().color = Color.yellow;
        }
        else
        {
            entryTransform.Find("PuntuaciónEntrada").GetComponent<Text>().color = Color.black;
            entryTransform.Find("NombreEntrada").GetComponent<Text>().color = Color.black;
            entryTransform.Find("PuestoEntrada").GetComponent<Text>().color = Color.black;
        }

        transformList.Add(entryTransform);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Inicio");
    }

    [System.Serializable]
    private class Highscores
    {
        public List<int> Scores;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
    }
}
