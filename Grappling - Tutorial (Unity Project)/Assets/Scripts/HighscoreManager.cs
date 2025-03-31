using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreManager : MonoBehaviour
{
    public InputField nameInput;
    public InputField passwordInput;
    public Text messageText;
    public Text highscoreText;

    private string playerName;
    private string playerPassword;
    private int playerScore = 0;

    public string serverUrl = "https://safe-epic-loon.ngrok-free.app";

    // **Benutzer registrieren**
    public void Register()
    {
        StartCoroutine(RegisterUser());
    }

    IEnumerator RegisterUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("player", nameInput.text);
        form.AddField("password", passwordInput.text);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl + "/register", form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                messageText.text = "Registrierung erfolgreich!";
            }
            else
            {
                messageText.text = "Fehler bei der Registrierung: " + request.downloadHandler.text;
            }
        }
    }

    // **Login**
    public void Login()
    {
        StartCoroutine(LoginUser());
    }

    IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("player", nameInput.text);
        form.AddField("password", passwordInput.text);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl + "/login", form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                playerName = nameInput.text;
                playerPassword = passwordInput.text;
                messageText.text = "Login erfolgreich!";
                
                yield return new WaitForSeconds(1); // 1 Sekunde warten, um das Login zu bestätigen
                SceneManager.LoadScene("GameScene"); // Szene wechseln
            }
            else
            {
                messageText.text = "Login fehlgeschlagen: " + request.downloadHandler.text;
            }
        }
    }

    // **Score speichern**
    public void SaveScore(int score)
    {
        playerScore = score;
        StartCoroutine(UpdateScore());
    }

    IEnumerator UpdateScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("player", playerName);
        form.AddField("password", playerPassword);
        form.AddField("score", playerScore.ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl + "/updateScore", form))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                messageText.text = "Score aktualisiert!";
            }
            else
            {
                messageText.text = "Fehler beim Aktualisieren des Scores: " + request.downloadHandler.text;
            }
        }
    }

    // **Highscores abrufen**
    public void LoadHighscores()
    {
        StartCoroutine(GetHighscores());
    }

    IEnumerator GetHighscores()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl + "/highscores"))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                highscoreText.text = request.downloadHandler.text;
            }
            else
            {
                highscoreText.text = "Fehler beim Abrufen der Highscores: " + request.downloadHandler.text;
            }
        }
    }
}
