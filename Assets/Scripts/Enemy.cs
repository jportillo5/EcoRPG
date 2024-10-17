using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    public float health = 1;
    public float followRange = 5f;  // Range within which the enemy will follow the player
    public float moveSpeed = 2f;    // Speed at which the enemy will move towards the player
    public float knockbackForce = 5f;  // Regular knockback force (for player collision)
    public float strongKnockbackForce = 10f;  // Stronger knockback force (for sword hits)
    public float knockbackDuration = 0.2f;  // Duration of the knockback effect
    public float knockbackRecoveryDelay = 1f;  // Time to wait after knockback before following the player again
    public float wanderDistance = 2f;  // Distance to wander in a random direction
    public float wanderDelay = 2f;  // Delay between random wander movements
    public float cooldownBeforeWander = 2f;  // Time to wait before wandering again after stopping following
    //public GameObject floatingTextPrefab;  // Prefab for floating text (drag this into the inspector)


    private Transform player;  // Reference to the player's transform
    private Vector2 wanderTarget;  // Randomly chosen target position to move to
    private bool isWandering = false;  // Flag to control wandering behavior
    private Coroutine wanderCoroutine;  // Store reference to the active Wander coroutine
    private bool isOnCooldown = false;  // Flag to check if in cooldown before wandering
    private bool isKnockedBack = false;  // Flag to prevent movement during knockback
    private bool damageLocked = false; // Flag to check if the enemy has "invulnerability frames"

    // Health property with getter and setter
    public float Health {
        set {
            health = value;
            if (health <= 0) {
                Defeated();
            }
        }
        get {
            return health;
        }
    }

    private void Start() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Initialize the SpriteRenderer component
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D for physics

        // Find the player in the scene by its tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (health > 0 && player != null && !isKnockedBack) {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= followRange) {
                if (isWandering) {
                    StopWandering();  // Stop wandering when starting to follow the player
                }
                FollowPlayer();
            } else {
                // Wander after cooldown if not following the player
                if (!isWandering && !isOnCooldown) {
                    StartCoroutine(CooldownBeforeWandering());
                }
            }
        }
    }

    // Method to move towards the player and handle animations
    void FollowPlayer() {
        Vector2 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Determine which direction to trigger animations for
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            // Horizontal movement (left/right)
            animator.Play("Slime_Walking_Left_Right");
            spriteRenderer.flipX = direction.x < 0;  // Flip the sprite based on direction
        } else {
            // Vertical movement (up/down)
            animator.Play(direction.y > 0 ? "Slime_Walking_Up" : "Slime_Walking_Down");
        }
    }

    // Coroutine to handle random wandering behavior
    IEnumerator Wander() {
        isWandering = true;

        // Choose a random direction to wander in (up, down, left, or right)
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 randomDirection = directions[Random.Range(0, directions.Length)];

        // Set a wander target position based on the random direction and wander distance
        wanderTarget = (Vector2)transform.position + randomDirection * wanderDistance;

        // Move towards the wander target
        while (Vector2.Distance(transform.position, wanderTarget) > 0.1f) {
            transform.position = Vector2.MoveTowards(transform.position, wanderTarget, moveSpeed * Time.deltaTime);

            // Determine which direction to trigger animations for
            if (Mathf.Abs(randomDirection.x) > Mathf.Abs(randomDirection.y)) {
                // Horizontal movement
                animator.Play("Slime_Walking_Left_Right");
                spriteRenderer.flipX = randomDirection.x < 0;  // Flip the sprite based on direction
            } else {
                // Vertical movement
                animator.Play(randomDirection.y > 0 ? "Slime_Walking_Up" : "Slime_Walking_Down");
            }

            yield return null;  // Wait until the next frame to continue moving
        }

        // After reaching the target, pause for a short delay before wandering again
        yield return new WaitForSeconds(wanderDelay);
        isWandering = false;
    }

    // Stop the wandering coroutine when the slime starts following the player
    void StopWandering() {
        if (wanderCoroutine != null) {
            StopCoroutine(wanderCoroutine);  // Stop the wandering coroutine
            isWandering = false;  // Reset the wandering flag
        }
    }

    // Cooldown before the slime starts wandering again after following the player
    IEnumerator CooldownBeforeWandering() {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownBeforeWander);  // Wait for the cooldown period
        isOnCooldown = false;  // Reset cooldown flag
        wanderCoroutine = StartCoroutine(Wander());  // Start wandering after cooldown
    }

    // Knockback the slime when hit by the player or when it hits the player
    public void ApplyKnockback(Vector2 direction, float knockbackForce) {
        if (!isKnockedBack) {
            StartCoroutine(KnockbackRoutine(direction, knockbackForce));
        }
    }

    // Coroutine to apply knockback, wait, and then recover
    IEnumerator KnockbackRoutine(Vector2 direction, float knockbackForce) {
        isKnockedBack = true;  // Disable movement during knockback

        // Apply knockback force
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Wait for the knockback duration
        yield return new WaitForSeconds(knockbackDuration);

        // Stop the knockback by resetting the velocity
        rb.velocity = Vector2.zero;

        // Wait for the knockback recovery delay before resuming behavior
        yield return new WaitForSeconds(knockbackRecoveryDelay);

        isKnockedBack = false;  // Re-enable movement
    }

    // Called when the slime collides with the player or something that should cause knockback
   private void OnCollisionEnter2D(Collision2D collision) {
    Debug.Log("Collision with: " + collision.gameObject.name);  // Log what the slime collided with

    if (collision.gameObject.CompareTag("Player")) {
        Debug.Log("Player collision detected.");
        
        Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
        ApplyKnockback(knockbackDirection, knockbackForce);  // Regular knockback for player collision

        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) {
            player.TakeDamage(10f);  // Deal damage
            Debug.Log("Player took damage!");
        }
    }
}



    public void TakeDamage(float damage) {
        // Apply damage to the slime's health
        if(!damageLocked) {
            Health -= damage;
            //Debug.Log("Slime damaged");
            if (Health <= 0) {
                Defeated();
            }
        }
    }



    // Called when health reaches 0 or below
    public void Defeated() {
        animator.Play("Slime_Death");  // Play death animation directly
        moveSpeed = 0f;  // Stop all movement after defeat
        Invoke("RemoveEnemy", .5f);
    }

    // Call this to remove the enemy after defeat animation
    public void RemoveEnemy() {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Weapon")) {
            TakeDamage(1f); //replace with a call to the weapon's power
            //no need to lock damage, the "cooldown" for using the weapon itself should be good enough
        } else if(other.gameObject.CompareTag("Spell")) { //I can't get the exploding projectile to work as intended, so this block currently isn't functional
            other.gameObject.GetComponent<SpellAttack>().explode();
            TakeDamage(other.gameObject.GetComponent<SpellAttack>().getPower());
            //figure out how to deal knockback
            damageLocked = true;
            Invoke("unlockDamage", other.gameObject.GetComponent<SpellAttack>().getDamageCooldown());
        }
    }

    void unlockDamage() {
        damageLocked = false;
    }
}
