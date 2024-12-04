using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAudio : MonoBehaviour
{
    public AudioClip attackSound;    // Assign the attack sound effect
    public AudioClip deathSound;     // Assign the death sound effect
    public AudioClip damageSound;    // Assign the damage sound effect
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    public void PlayDamageSound()
    {
        
        if (damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }
}
