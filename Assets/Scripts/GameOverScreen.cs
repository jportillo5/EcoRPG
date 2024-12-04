using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    Player player;
    LevelLoader ll;

    public void Start() {
        player = FindObjectOfType<Player>();
        ll = FindObjectOfType<LevelLoader>();
    }

    public void Setup() {
        gameObject.SetActive(true);
    }

    public void RestartButton() {
        ll.LoadNxtLevel(3);
        //SceneManager.LoadScene(0);
        ResetScene();
    }
    public void ExitButton() {
        //Time.timeScale = 0;
        //SceneManager.LoadScene("MainMenu");
        //ll.LoadNxtLevel(0);
    }

    public void ResetScene()
    {
        // Reloads the current scene
        player.Reset();
    }
}
