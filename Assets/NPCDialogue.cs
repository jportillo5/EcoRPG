using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject npcNameObject;  // Reference to the NPCName GameObject
    public GameObject npcImageObject; // Reference to the NPCImage GameObject
    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public Button contButton;  // Changed to Button type for onClick event handling
    public float wordSpeed;
    public bool playerIsClose;

    public string npcName;    // NPC's name (to customize per NPC)
    public Sprite npcSprite;  // NPC's image/portrait (to customize per NPC)

    private Text npcNameText; // Text component of the NPCName GameObject
    private Image npcImage;   // Image component of the NPCImage GameObject

    void Start()
    {
        if (dialoguePanel == null) Debug.LogError("Dialogue Panel is not assigned in the Inspector.");
        if (dialogueText == null) Debug.LogError("Dialogue Text is not assigned in the Inspector.");
        if (contButton == null) Debug.LogError("Continue Button is not assigned in the Inspector.");
        if (npcNameObject == null) Debug.LogError("NPC Name GameObject is not assigned in the Inspector.");
        if (npcImageObject == null) Debug.LogError("NPC Image GameObject is not assigned in the Inspector.");

        // Get the Text and Image components from the NPCName and NPCImage GameObjects
        npcNameText = npcNameObject.GetComponent<Text>();
        npcImage = npcImageObject.GetComponent<Image>();

        // Make sure the button is initially disabled
        contButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                // Set NPC name and image when the dialogue starts
                npcNameText.text = npcName;      // Update NPC name
                npcImage.sprite = npcSprite;     // Update NPC portrait

                // Enable the dialogue panel
                dialoguePanel.SetActive(true);

                // Start typing dialogue
                StartCoroutine(Typing());

                // Dynamically assign the onClick event of the continue button
                contButton.onClick.RemoveAllListeners();  // Clear previous listeners
                contButton.onClick.AddListener(NextLine); // Assign this NPC's NextLine method
                contButton.gameObject.SetActive(false);   // Hide the button until typing is done
            }
        }

        // Show the continue button when the dialogue finishes typing
        if (dialogueText.text == dialogue[index])
        {
            contButton.gameObject.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.gameObject.SetActive(false); // Hide the button while processing
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
