using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class FinalScore : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager score;

    string filePath;

    // Clase auxiliar para la serialización
    [System.Serializable]
    private class ScoreData
    {
        public List<int> Scores = new List<int>();
    }

    void Start()
    {
        // Define la ruta del archivo en persistentDataPath
        filePath = "Assets/Scripts/Highscore/ScoreData.json";

        if (player != null)
        {
            player.OnGameOver += HighscoreScene;
        }
        else
        {
            Debug.LogError("Player no asignado");
        }
    }

    private void HighscoreScene()
    {
        // Leer los puntajes existentes desde el archivo
        ScoreData scoreData = new ScoreData();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scoreData = JsonUtility.FromJson<ScoreData>(json);
        }

        // Agregar el puntaje actual a la lista
        scoreData.Scores.Add(score.Score);

        try
        {
            // Convertir la lista actualizada a JSON y guardarla en el archivo
            string jsonScoreData = JsonUtility.ToJson(scoreData);
            File.WriteAllText(filePath, jsonScoreData);
            Debug.Log("Datos Guardados en " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar los datos: " + e.Message);
        }

        SceneManager.LoadScene("Highscore");
    }
}
