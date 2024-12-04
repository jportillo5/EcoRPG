using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    //After 70 Seconds, the scene will switch to the next scene
    public float timeToSwitch = 70f;
    public string sceneName = "FarmScene";




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchScene());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(timeToSwitch);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
