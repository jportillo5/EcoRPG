using System.Collections;
using UnityEngine;

public class NPCAnimalWander : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    public float moveSpeed = 3f;          // Movement speed of the animal
    public float wanderDistance = 3f;   // Distance to wander in a random direction
    public float wanderDelay = 5f;        // Delay between random wander movements

    private Vector2 wanderTarget;         // Randomly chosen target position to move to
    private bool isWandering = false;     // Flag to control wandering behavior
    private Coroutine wanderCoroutine;    // Store reference to the active Wander coroutine

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

    // Coroutine to handle random wandering behavior
    IEnumerator Wander()
    {
        isWandering = true;

        // Choose a random direction to wander in (up, down, left, or right)
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 randomDirection = directions[Random.Range(0, directions.Length)];

        // Set a wander target position based on the random direction and wander distance
        wanderTarget = (Vector2)transform.position + randomDirection * wanderDistance;

        // Move towards the wander target
        while (Vector2.Distance(transform.position, wanderTarget) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, wanderTarget, moveSpeed * Time.deltaTime);

            // Play the appropriate animation based on movement direction
            if (Mathf.Abs(randomDirection.x) > Mathf.Abs(randomDirection.y))
            {
                // Horizontal movement (left/right)
                animator.Play("Animal_Walking_Left_Right");
                spriteRenderer.flipX = randomDirection.x < 0;  // Flip the sprite based on direction
            }
            else
            {
                // Vertical movement (up/down)
                animator.Play(randomDirection.y > 0 ? "Animal_Walking_Up" : "Animal_Walking_Down");
            }

            yield return null;  // Wait until the next frame to continue moving
        }

        // Set the animal to idle after reaching the target
        animator.Play("Animal_Idle");

        // Wait for the specified wander delay before choosing a new direction
        yield return new WaitForSeconds(wanderDelay);

        isWandering = false;
        wanderCoroutine = null;  // Reset coroutine reference to allow restarting the wander
    }
}
