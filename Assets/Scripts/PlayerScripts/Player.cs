using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour
{
    public event Action OnPlayerDamaged;
    public event Action OnPlayerDeath;

    public float health, maxHealth;
    public float maxMP;
    public float mpRecoveryRate; //amount of MP meant to be recovered in one second
    public AudioClip damageClip;
    float currentMP;

    private MPBarController mpBar;
    AudioSource myAudio;


    void Start() {
        health = maxHealth;
        Debug.Log("Player starts with " + health + " health.");
        currentMP = maxMP;
        mpBar = GameObject.Find("MPBar").GetComponent<MPBarController>();
        myAudio = GetComponent<AudioSource>();
    }
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        myAudio.PlayOneShot(damageClip);
        Debug.Log("Player took " + damage + " damage. Current health: " + health);
        OnPlayerDamaged?.Invoke();

        if (health <= 0)
        {
            Die();
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

    public void Heal(float healing) {
        //Debug.Log("healing method called successfully");
        health += healing;
        if(health >= maxHealth) {
            health = maxHealth;
        }
        Debug.Log("current health" + health + "/" + maxHealth);
    }

    public float getCurrentHP() {
        return health;
    }

    public float getMaxHP() {
        return maxHealth;
    }

    public float getMaxMP() {
        return maxMP;
    }

    public void depleteMP(float mp) {
        currentMP -= mp;
        if(currentMP <= 0) {
            currentMP = 0;
        }
        mpBar.UpdateMPBar(currentMP);
    }

    public void recoverMP(float mp) {
        currentMP += mp;
        if(currentMP >= maxMP) {
            currentMP = maxMP;
        }
        mpBar.UpdateMPBar(currentMP);
    }

    void autoRecoverMP() {
        Debug.Log("autoRecoverMP call successful");
        currentMP += mpRecoveryRate/60f;
        if(currentMP >= maxMP) {
            currentMP = maxMP;
        }
        Debug.Log("Attempting to call to update MP Bar");
        mpBar.UpdateMPBar(currentMP);
        Debug.Log("CurrentMP " + currentMP + "/" + maxMP);
        Debug.Log("MP Bar State: " + mpBar.getState());
    }

    public float getCurrentMP() {
        return currentMP;
    }

    public void BeginMPRecovery() {
        Debug.Log("BeginMPRecovery Call successful");
        if(currentMP < maxMP) {
            float rate = 1f/60f;
            Invoke("autoRecoverMP", rate); //setup needs to be reworked for a smoother transition on the MP recovery
        }
    }

    public void setVolume(float volume) {
        myAudio.volume = volume;
    }

    public void playClip(AudioClip clip) {
        myAudio.PlayOneShot(clip);
    }
}
