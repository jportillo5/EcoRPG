using UnityEngine;

public class HighlightableItem : MonoBehaviour
{
    public Color highlightColor = Color.yellow;  // The color to highlight the item with
    private Color originalColor;                 // Stores the original color of the item
    private SpriteRenderer spriteRenderer;       // Reference to the SpriteRenderer

    private void Start()
    {
        // Get the SpriteRenderer component from the item
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original color of the sprite
        originalColor = spriteRenderer.color;
    }

    // Method to highlight the item by changing its color
    public void Highlight()
    {
        spriteRenderer.color = highlightColor;
    }

    // Method to revert the item to its original color
    public void Unhighlight()
    {
        spriteRenderer.color = originalColor;
    }

    // Method to handle item interaction (triggered when player presses a button, for example)
    public void Interact()
    {
        Debug.Log("Item interaction with " + spriteRenderer.name + " was successful!");
    }
}
