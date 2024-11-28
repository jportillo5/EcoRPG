using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        //     LoadNextLevel();
        // }
    }

    public void LoadNextLevel() {
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator loadLevel(int levelIndex) {
        // play animation
        transition.SetTrigger("Start");
        // wait
        yield return new WaitForSeconds(transitionTime);
        // load the scene
        SceneManager.LoadScene(levelIndex);
    }
}
