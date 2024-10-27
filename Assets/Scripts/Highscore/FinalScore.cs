using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class FinalScore : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager score;

    // Variable estática para almacenar el puntuaje de la partida actual
    public static int currentScore;

    void Start()
    {
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
        // Guardar el puntuaje actual en la variable 
        currentScore = score.Score;

        SceneManager.LoadScene("Highscore");
    }
}
