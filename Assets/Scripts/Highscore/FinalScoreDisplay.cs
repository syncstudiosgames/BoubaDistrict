using UnityEngine;
using TMPro;

public class FinalScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText; 

    private void Start()
    {
        // Obtener la puntuación final desde FinalScore
        int score = FinalScore.finalScore;

        UpdateFinalScoreUI(score);
    }

    private void UpdateFinalScoreUI(int score)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = $"YOUR SCORE: {score}";
        }
        else
        {
            Debug.LogError("FinalScoreText no asignado en el Inspector.");
        }
    }
}
