using System.Collections.Generic;
using UnityEngine;

public class SaveScore : MonoBehaviour
{
    public Score scoredata = new Score();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveToJson();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJson();
        }
    }
    public void SaveToJson()
    {
        string scoreData = JsonUtility.ToJson(scoredata);
        string filePath = "Assets/Scripts/Highscore/ScoreData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, scoreData);
        Debug.Log("Datos Guardados");
    }

    public void LoadFromJson()
    {
        string filePath = "Assets/Scripts/Highscore/ScoreData.json";
        string scoreData = System.IO.File.ReadAllText(filePath);

        scoredata = JsonUtility.FromJson<Score>(scoreData);
        Debug.Log("Archivo leido");
    }
}

[System.Serializable]
public class Score
{
    public List<Scores> scoreList = new List<Scores>();
}

[System.Serializable]
public class Scores
{
    public int score;
    public string name;
}


