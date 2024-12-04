using System.Collections;
using UnityEngine;

public class NPCAnimalWander : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    public float moveSpeed = 3f;          // Movement speed of the animal
    public float wanderDistance = 3f;    // Distance to wander in a random direction
    public float wanderDelay = 5f;       // Delay between random wander movements

    private Vector2 wanderTarget;        // Randomly chosen target position to move to
    private bool isWandering = false;    // Flag to control wandering behavior
    private Coroutine wanderCoroutine;   // Store reference to the active Wander coroutine
    private Vector2 currentDirection;    // Current movement direction
    private bool hasCollided = false;    // Flag for handling collisions

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Start the wandering coroutine
        wanderCoroutine = StartCoroutine(Wander());
    }

    void Update()
    {
        // Ensure that the NPC wanders continuously after the delay
        if (!isWandering && wanderCoroutine == null)
        {
            wanderCoroutine = StartCoroutine(Wander());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != gameObject) // Ignore self-collision
        {
            hasCollided = true;
            StopCoroutine(wanderCoroutine);
            StartCoroutine(HandleCollision());
        }
    }

    // Coroutine to handle random wandering behavior
    IEnumerator Wander()
    {
        isWandering = true;

        // Choose a random direction to wander in (up, down, left, or right)
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        currentDirection = directions[Random.Range(0, directions.Length)];

        // Set a wander target position based on the random direction and wander distance
        wanderTarget = (Vector2)transform.position + currentDirection * wanderDistance;

        // Move towards the wander target
        while (Vector2.Distance(transform.position, wanderTarget) > 0.1f && !hasCollided)
        {
            transform.position = Vector2.MoveTowards(transform.position, wanderTarget, moveSpeed * Time.deltaTime);

            // Play the appropriate animation based on movement direction
            if (Mathf.Abs(currentDirection.x) > Mathf.Abs(currentDirection.y))
            {
                // Horizontal movement (left/right)
                animator.Play("Animal_Walking_Left_Right");
                spriteRenderer.flipX = currentDirection.x < 0;  // Flip the sprite based on direction
            }
            else
            {
                // Vertical movement (up/down)
                animator.Play(currentDirection.y > 0 ? "Animal_Walking_Up" : "Animal_Walking_Down");
            }

            yield return null;  // Wait until the next frame to continue moving
        }

        if (!hasCollided)
        {
            // Set the animal to idle after reaching the target
            animator.Play("Animal_Idle");

            // Wait for the specified wander delay before choosing a new direction
            yield return new WaitForSeconds(wanderDelay);
        }

        isWandering = false;
        wanderCoroutine = null;  // Reset coroutine reference to allow restarting the wander
    }

    // Handle collision behavior
    IEnumerator HandleCollision()
    {
        // Stop moving and play idle animation
        animator.Play("Animal_Idle");

        // Move slightly backward to avoid the obstacle
        Vector2 backStep = (Vector2)transform.position - currentDirection * 0.5f;
        transform.position = Vector2.MoveTowards(transform.position, backStep, moveSpeed * Time.deltaTime);

        // Wait for the wander delay
        yield return new WaitForSeconds(wanderDelay);

        // Reset collision flag and start wandering again
        hasCollided = false;
        wanderCoroutine = StartCoroutine(Wander());
    }
}
