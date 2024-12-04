using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;
    Player player;

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        //     LoadNextLevel();
        // }
    }

    void Start() {
        player = FindObjectOfType<Player>();
    }

    public void LoadNextLevel(bool dead) {
        if (dead == false) {
            StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        } else {
            StartCoroutine(loadLevel(3));
        }
        
    }

    public void LoadNxtLevel(int i) {
        
        StartCoroutine(loadLevel(i));
        
    }

    IEnumerator loadLevel(int levelIndex) {
        // play animation
        transition.SetTrigger("Start");
        // wait
        yield return new WaitForSeconds(transitionTime);
        // load the scene
        SceneManager.LoadScene(levelIndex);
        // if(levelIndex == 3) {
        //     player.health = player.maxHealth;
        // }
    }

    
}
