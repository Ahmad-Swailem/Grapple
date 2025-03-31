using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class HighscoreFetcher : MonoBehaviour
{
    public Text scoreText; // Legacy UI Text-Element (nicht TextMeshPro!)
    private List<Highscore> players = new List<Highscore>();
    public string url = "https://safe-epic-loon.ngrok-free.app/highscores";

    void Start()
    {
        StartCoroutine(GetHighScores());
    }

    IEnumerator GetHighScores()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"highscores\":" + request.downloadHandler.text + "}";

            var highscoreData = JsonUtility.FromJson<HighscoreArray>(json);
            if (highscoreData != null && highscoreData.highscores != null)
            {
                players = new List<Highscore>(highscoreData.highscores);
                players.Sort((a, b) => b.score.CompareTo(a.score));
            }
            else
            {
                Debug.LogError("Fehler: Highscore-Liste ist leer oder ungültig!");
                players = new List<Highscore>();
            }

            UpdateUIText();
        }
        else
        {
            Debug.LogError("Fehler beim Abrufen der Highscores: " + request.error);
        }
    }

    void UpdateUIText()
    {
        if (scoreText == null)
        {
            Debug.LogError("Fehler: `scoreText` ist nicht zugewiesen!");
            return;
        }

        scoreText.text = "Highscores:\n";
        foreach (var player in players)
        {
            scoreText.text += $"{player.player} - {player.score}\n";
        }
    }
}

// Klasse für Highscores
[System.Serializable]
public class Highscore
{
    public string player;
    public int score;
    public int ID;
}

// Klasse für JSON Parsing
[System.Serializable]
public class HighscoreArray
{
    public Highscore[] highscores;
}
