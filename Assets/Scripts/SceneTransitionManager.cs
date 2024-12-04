using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene to load
    public Vector2 entryPoint; // Position to place the player in the next scene
    LevelLoader ll;

    

    // Trigger for entering a new scene
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Save the entry point position
            PlayerPrefs.SetFloat("PlayerX", entryPoint.x);
            PlayerPrefs.SetFloat("PlayerY", entryPoint.y);

            // Load the new scene
            //SceneManager.LoadScene(sceneToLoad);
            ll.LoadNextLevel(false);

        }
    }

    // Function to place the player at the saved position in the new scene
    private void Start()
    {
        ll = FindObjectOfType<LevelLoader>();

        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                player.transform.position = new Vector2(x, y);
            }
        }
    }
}
