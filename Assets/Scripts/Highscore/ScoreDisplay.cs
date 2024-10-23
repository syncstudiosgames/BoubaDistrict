using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] ScoreManager _scoreManager;

    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] TextMeshProUGUI comboDisplay;

    private void Start()
    {
        _scoreManager.OnScoreChanged += UpdateScore;
        _scoreManager.OnComboChanged += UpdateCombo;

        UpdateScore();
        UpdateCombo();
    }

    void UpdateScore()
    {
        scoreDisplay.text = _scoreManager.Score.ToString();
    }
    void UpdateCombo()
    {
        comboDisplay.text = _scoreManager.Combo.ToString();
    }
}
