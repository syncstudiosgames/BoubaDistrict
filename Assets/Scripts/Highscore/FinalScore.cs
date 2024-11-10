using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FinalScore : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager score;
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
        currentScore = score.Score;

        string playerName = "Juan"; //
        //string playerName = player.GetPlayerName(); 
        StartCoroutine(SendScoreAndLoadHighscore(playerName, currentScore));
    }

    private IEnumerator SendScoreAndLoadHighscore(string playerName, int score)
    {
        yield return StartCoroutine(SendScoreToServer(playerName, score));

        SceneManager.LoadScene("Highscore");
    }

    // Enviar la puntuación al servidor
    private IEnumerator SendScoreToServer(string playerName, int score)
    {
        // Crear los datos en formato JSON
        string jsonData = JsonUtility.ToJson(new HighscoreEntry { name = playerName, score = score });

        // Solicitud POST
        using (UnityWebRequest request = new UnityWebRequest("https://highscore-server.glitch.me/api/submit-score", UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Puntuación enviada correctamente.");
            }
            else
            {
                Debug.LogError($"Error al enviar puntuación: {request.error}");
            }
        }
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public string name;
        public int score;
    }
}
