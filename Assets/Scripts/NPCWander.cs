using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWander : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed at which NPC moves
    public float waitTime = 2f;  // Time to wait before moving to the next point
    public GameObject boundsObject; // Reference to the bounds object with BoxCollider2D
    
    private Vector2 minBounds;  // Bottom-left corner of the bounds
    private Vector2 maxBounds;  // Top-right corner of the bounds
    private Vector2 targetPosition; // The current target position to move towards
    private bool isMoving = false;
    private Vector2 currentDirection;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get the bounds from the BoxCollider2D
        BoxCollider2D boundsCollider = boundsObject.GetComponent<BoxCollider2D>();
        minBounds = boundsCollider.bounds.min;
        maxBounds = boundsCollider.bounds.max;

        // Start wandering
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (true)
        {
            if (!isMoving)
            {
                // Pick a random direction: left (x=-1), right (x=1), up (y=1), down (y=-1)
                int direction = Random.Range(0, 4); // 0 = left, 1 = right, 2 = up, 3 = down
                switch (direction)
                {
                    case 0: // Move left
                        currentDirection = Vector2.left;
                        targetPosition = new Vector2(minBounds.x, transform.position.y);
                        break;
                    case 1: // Move right
                        currentDirection = Vector2.right;
                        targetPosition = new Vector2(maxBounds.x, transform.position.y);
                        break;
                    case 2: // Move up
                        currentDirection = Vector2.up;
                        targetPosition = new Vector2(transform.position.x, maxBounds.y);
                        break;
                    case 3: // Move down
                        currentDirection = Vector2.down;
                        targetPosition = new Vector2(transform.position.x, minBounds.y);
                        break;
                }

                isMoving = true;

                // Move towards the target position
                yield return StartCoroutine(MoveToTarget());

                // Wait before picking a new target
                yield return new WaitForSeconds(waitTime);
            }
        }
    }

    IEnumerator MoveToTarget()
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Only move in straight lines (either x or y) at a time
            Vector2 newPosition = rb.position + currentDirection * moveSpeed * Time.deltaTime;
            
            // Ensure NPC stays within bounds (manual clamping)
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

            rb.MovePosition(newPosition); // Apply the movement
            yield return null;
        }

        isMoving = false;
    }

    void OnDrawGizmos()
    {
        if (boundsObject != null)
        {
            // Draw bounds in the editor for visualization
            BoxCollider2D boundsCollider = boundsObject.GetComponent<BoxCollider2D>();
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boundsCollider.bounds.center, boundsCollider.bounds.size);
        }
    }
}
