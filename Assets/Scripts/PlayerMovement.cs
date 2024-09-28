using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement
    public Rigidbody2D rb; // Reference to the player's Rigidbody2D component

    private Vector2 movement; // Store the player's movement direction

    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrow keys
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down arrow keys
    }

    void FixedUpdate()
    {
        // Apply movement to the player's Rigidbody2D
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
