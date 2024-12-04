using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    private GameObject dialoguePanel;
    private GameObject npcNameObject;
    private GameObject npcImageObject;
    private Text dialogueText;  // Changed to private since it's now found in Start()
    private Button contButton;   // Changed to private since it's now found in Start()
    public string[] dialogue;
    private int index;
    public float wordSpeed = 0.8f;
    public bool playerIsClose;

    public string npcName;    // NPC's name (to customize per NPC)
    public Sprite npcSprite;  // NPC's image/portrait (to customize per NPC)

    private Text npcNameText;
    private Image npcImage;

    void Start()
    {
        // Find the Canvas in the scene, which is the parent of the DialoguePanel
        GameObject canvas = GameObject.Find("Canvas");

        // Locate DialoguePanel within the Canvas object
        dialoguePanel = canvas.transform.Find("DialoguePanel")?.gameObject;

        // Locate the Text component for displaying dialogue within the DialoguePanel
        dialogueText = dialoguePanel.transform.Find("DialogueText")?.GetComponent<Text>();

        // Locate the ContinueButton within the DialoguePanel and get its Button component
        contButton = dialoguePanel.transform.Find("ContinueButton")?.GetComponent<Button>();

        // Find the NPCName GameObject under DialoguePanel, which displays the NPC's name
        npcNameObject = dialoguePanel.transform.Find("NPCName")?.gameObject;

        // Find the NPCImage GameObject under DialoguePanel, which displays the NPC's image
        npcImageObject = dialoguePanel.transform.Find("NPCImage")?.gameObject;

        // Get the Text component from NPCName to set the NPC's name during dialogue
        npcNameText = npcNameObject.GetComponent<Text>();

        // Get the Image component from NPCImage to set the NPC's portrait
        npcImage = npcImageObject.GetComponent<Image>();

        // Initially, hide the ContinueButton until the dialogue text has finished typing
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
            npcNameText.text = npcName;
            npcImage.sprite = npcSprite;

            dialoguePanel.SetActive(true);
            StartCoroutine(Typing());

            contButton.onClick.RemoveAllListeners();
            contButton.onClick.AddListener(NextLine);
            contButton.gameObject.SetActive(false);
        }
    }

    // Check if the index is within bounds before accessing dialogue[index]
    if (index < dialogue.Length && dialogueText.text == dialogue[index])
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
        contButton.gameObject.SetActive(false);
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
