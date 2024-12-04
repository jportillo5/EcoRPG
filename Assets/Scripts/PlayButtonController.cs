using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonController : MonoBehaviour
{

    public GameObject crossFade;
    public Button PlayButton;
    public string sceneName;

    Animator crossFadeAnimator;

    // Start is called before the first frame update
    void Start()
    {   
        crossFadeAnimator = crossFade.GetComponent<Animator>();
        PlayButton = GetComponent<Button>();
        PlayButton.onClick.AddListener(PlayButtonClicked);
    }


    void PlayButtonClicked()
    {
        crossFadeAnimator.Play("Close");
        //Make the game wait for 1 second before loading the next scene
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
