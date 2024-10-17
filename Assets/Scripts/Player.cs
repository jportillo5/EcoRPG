using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    public event Action OnPlayerDamaged;
    public event Action OnPlayerDeath;

    public float health, maxHealth;

    void Start() {
        health = maxHealth;
        Debug.Log("Player starts with " + health + " health.");
    }
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + health);
        OnPlayerDamaged?.Invoke();

        if (health <= 0)
        {
            //Die();
            Destroy(gameObject);
            //Debug.Log("Player has died.");
            Debug.Log("Player took " + damage + " damage. Current health: " + health);
        }
    }

    void Die()
    {
        Destroy(gameObject);
        //Debug.Log("Player has died.");
        //Debug.Log("Player took " + damage + " damage. Current health: " + health);
        //OnPlayerDeath?.Invoke();
        // Handle player death (e.g., restart the level, trigger death animation, etc.)
    }
}
