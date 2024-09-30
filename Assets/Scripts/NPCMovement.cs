using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public float idleTime = 3f; // Time to be idle before changing direction
    public float detectionRadius = 5f; // Range at which the NPC detects the player
    public LayerMask playerLayer; // Layer for detecting the player

    private Rigidbody2D rb;
    
    private Vector2 moveDirection;
    private float idleTimer;
    private Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        idleTimer = idleTime;
        ChooseRandomDirection();
        
    }

    void Update()
    {
        // Check if the player is within range
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        
        if (player != null)
        {
            // If the player is within range, start chasing
            playerTransform = player.transform;
            ChasePlayer();
        }
        else
        {
            // Random walking when idle
            IdleWalk();
        }
    }

    void IdleWalk()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            // Change direction every few seconds
            ChooseRandomDirection();
            idleTimer = idleTime;
        }

        // Set the velocity to move in the chosen direction
        rb.velocity = moveDirection * speed;
    }

    void ChasePlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void ChooseRandomDirection()
    {
        // Random direction (up, down, left, right)
        int randomDir = Random.Range(0, 4);
        switch (randomDir)
        {
            case 0: moveDirection = Vector2.up; break;
            case 1: moveDirection = Vector2.down; break;
            case 2: moveDirection = Vector2.left; break;
            case 3: moveDirection = Vector2.right; break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
