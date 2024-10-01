using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        Debug.Log("Player has died.");
        // Handle player death (e.g., restart the level, trigger death animation, etc.)
    }
}
