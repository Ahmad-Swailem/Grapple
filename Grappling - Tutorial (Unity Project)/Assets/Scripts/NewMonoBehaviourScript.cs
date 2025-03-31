using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{ 
    public Scene currentScene;
    void Update()
    {
        // Beispiel: Wenn die Q-Taste gedrückt wird, die Szene wechseln
        if (Input.GetKey(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
            currentScene = SceneManager.GetActiveScene();
            // Gib den Namen der aktuellen Szene aus
            Debug.Log("Aktuelle Szene: " + currentScene.name);
        }
    }
}
