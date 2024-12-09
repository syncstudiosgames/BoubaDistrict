using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FinalScore : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ScoreManager score;

    public static int finalScore; // Puntuación final
    public static string playerId; // ID del jugador

    private bool scoreSent = false;

    void Start()
    {
        if (player != null)
        {
            player.OnGameOver -= HighscoreScene;
            player.OnGameOver += HighscoreScene;
        }
        else
        {
            Debug.LogError("Player no asignado");
        }
    }

    private void HighscoreScene()
    {
        if (scoreSent) return;
        scoreSent = true;

        finalScore = score.Score;

        string playerName = player.GetPlayerName();
        int skinIndex = player.GetSkinIndex(); 

        StartCoroutine(SendScoreAndLoadHighscore(playerName, finalScore, skinIndex));
    }

    private IEnumerator SendScoreAndLoadHighscore(string playerName, int score, int skinIndex)
    {
        yield return StartCoroutine(SendScoreToServer(playerName, score, skinIndex));

        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator SendScoreToServer(string playerName, int score, int skinIndex)
    {
        var playerScore = new HighscoreEntry
        {
            name = playerName,
            score = score,
            characterIndex = skinIndex 
        };

        string jsonData = JsonUtility.ToJson(playerScore);

        Debug.Log("JSON enviado al servidor: " + jsonData); 

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
        public int characterIndex; 
    }

    [System.Serializable]
    private class PlayerIdResponse
    {
        public string id;
    }
}
