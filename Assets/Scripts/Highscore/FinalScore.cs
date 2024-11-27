using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FinalScore : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager score;
    public static int currentScore;
    public static string playerId; // ID del jugador
    private bool scoreSent = false; // Evitar envíos múltiples

    void Start()
    {
        if (player != null)
        {
            player.OnGameOver -= HighscoreScene; // Desuscribir cualquier suscripción previa
            player.OnGameOver += HighscoreScene; // Suscribir el evento
        }
        else
        {
            Debug.LogError("Player no asignado");
        }
    }

    private void HighscoreScene()
    {
        if (scoreSent) return; // Salir si ya se envió la puntuación
        scoreSent = true; // Marcar como enviada

        currentScore = score.Score;
        string playerName = player.GetPlayerName();
        StartCoroutine(SendScoreAndLoadHighscore(playerName, currentScore));
    }

    private IEnumerator SendScoreAndLoadHighscore(string playerName, int score)
    {
        yield return StartCoroutine(SendScoreToServer(playerName, score));

        SceneManager.LoadScene("Highscore");
    }

    private IEnumerator SendScoreToServer(string playerName, int score)
    {
        var playerScore = new HighscoreEntry
        {
            name = playerName,
            score = score
        };

        string jsonData = JsonUtility.ToJson(playerScore);

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
                string responseBody = request.downloadHandler.text;
                PlayerIdResponse response = JsonUtility.FromJson<PlayerIdResponse>(responseBody);
                playerId = response.id;
                Debug.Log("ID del jugador recibido: " + playerId);
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

    [System.Serializable]
    private class PlayerIdResponse
    {
        public string id; // ID del jugador
    }
}
