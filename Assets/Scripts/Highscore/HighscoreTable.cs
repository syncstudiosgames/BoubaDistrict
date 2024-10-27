using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        // Inicializamos la lista de puntuaciones ficticias
        highscoreEntryList = new List<HighscoreEntry>
        {
            new HighscoreEntry { score = 67 },
            new HighscoreEntry { score = 89 },
            new HighscoreEntry { score = 112 },
            new HighscoreEntry { score = 34 },
            new HighscoreEntry { score = 87 }
        };

        // Agregar el puntuaje de la partida actual desde FinalScore
        highscoreEntryList.Add(new HighscoreEntry { score = FinalScore.currentScore });

        // Marcar el último elemento añadido como el más reciente
        HighscoreEntry mostRecentEntry = highscoreEntryList[highscoreEntryList.Count - 1];

        // Ordenar la lista de puntuaciones
        highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            // Comprobar cual es el más reciente para cambiar su color
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

        string name = "AAA"; // Nombre 
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

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
    }
}
