using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEditor.Callbacks;
using UnityEngine.Android;

public class PlayerController : MonoBehaviour
{
    //Components Connected to this game object

    PlayerInput playerInput;
    SpriteRenderer mySpriteRenderer;
    Animator myAnim;
    Vector2 movementInput;
    Rigidbody2D myBod;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    WeaponController myWeapon;

    AudioSource myAudioSource;

    public AudioClip barkSound;
    private AudioClip miscAudio; //clip set by external sources with the "setAudio" method, and then played with the "playAudio" method

    //Components related to other game objects probably
    
    //Private properties
    private float[] lastInput;
    private string directionFacing;
    private bool strafing;
    private bool inputsLocked; //used to prevent inputs from being made in certain scenarios

    //public properties
    public float moveSpeed;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    public float movementPenalty;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        myAnim = GetComponent<Animator>();
        mySpriteRenderer = GameObject.Find("Item").GetComponent<SpriteRenderer>();
        lastInput = new float[] {0, 0}; //no input
        myBod = GetComponent<Rigidbody2D>();
        directionFacing = "down";
        myWeapon = GetComponentInChildren<WeaponController>();
        myAudioSource = GetComponent<AudioSource>();
        strafing = false;
    }

    void Update() {
        //Play Audioclip when player presses 'F'
        if (Keyboard.current.fKey.wasPressedThisFrame) {
            myAudioSource.PlayOneShot(barkSound);
        }

    }


    private void FixedUpdate() { //if regular update isn't necessary, move stuff from update to here ig?
        //If movement input is not 0, try to move, otherwise idle
        if(movementInput != Vector2.zero && !inputsLocked) {
            int count = myBod.Cast(
                movementInput,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
                );
            if(count == 0) {
                float movement = 1;
                if(strafing) {
                    movement = movementPenalty;
                }
                myBod.MovePosition(myBod.position + movementInput * moveSpeed * movement * Time.fixedDeltaTime);
                float h = movementInput[0];
                float v = movementInput[1];
                myAnim.SetBool("Moving", true);
                determineDirectionOfMovement(h, v);
            }
        } else {
            myAnim.SetBool("Moving", false);
            lastInput[0] = 0;
            lastInput[1] = 0;
        }
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnPause() {
        playerInput.SwitchCurrentActionMap("UI");
        GameObject.Find("PauseMenu").GetComponent<PauseMenuController>().openMenu();
    }

    public void regainPlayerControl() {
        playerInput.SwitchCurrentActionMap("Player");
    }

    void OnStrafe(InputValue strafeValue) {
        if (strafeValue.isPressed) {
            strafing = true;
        } else { //not pressed
            strafing = false;
        }
    }

    public void attack() {
        strafing = true;
        myAnim.SetBool("Attacking", true);
        Invoke("stopAttack", 0.2f);
    }

    

    private void determineDirectionOfMovement(float h, float v) { //more like direction of animation
        //last input same as new input.
        if(inputsLocked) {
            return;
        }
        if((lastInput[0] == h) && (lastInput[1] == v)) { 
            //change direction facing
            myAnim.SetBool("Moving", true);
            return;
        }
        if(strafing) { //do not attempt to change direction of animation
            return;
        }
        
        //last h == new h, last v != new v; prioritize vertical input, change direction facing first frame, do not move yet
        else if((lastInput[0] == h) && (lastInput[1] != v)) {
            lastInput[1] = v;
            //change animation state and apply motion
            switch(v) {
                case 1:
                    myAnim.SetBool("Up", true);
                    myAnim.SetBool("Down", false);
                    myAnim.SetBool("Left", false);
                    myAnim.SetBool("Right", false);
                    directionFacing = "up";
                    break;
                case -1:
                    myAnim.SetBool("Down", true);
                    myAnim.SetBool("Up", false);
                    myAnim.SetBool("Left", false);
                    myAnim.SetBool("Right", false);
                    directionFacing = "down";
                    break;
                default:
                    break;
            }
            //myAnim.SetBool("Moving", false);
            return;
        }

        //last h != new h, last v == new v; prioritize horizontal input, change direction facing first frame, do not move yet
        else if((lastInput[0] != h) && (lastInput[1] == v)) {
            lastInput[0] = h;
            //change animaion state and apply motion
            switch(h) {
                case 1:
                    myAnim.SetBool("Down", false);
                    myAnim.SetBool("Up", false);
                    myAnim.SetBool("Left", false);
                    myAnim.SetBool("Right", true);
                    directionFacing = "right";
                    break;
                case -1:     //h = -1, left
                    myAnim.SetBool("Down", false);
                    myAnim.SetBool("Up", false);
                    myAnim.SetBool("Left", true);
                    myAnim.SetBool("Right", false);
                    directionFacing = "left";
                    break;
                default:
                    break;    
            }
            //myAnim.SetBool("Moving", false);
            return;
        }

        else if((lastInput[0] != h) && (lastInput[1] != v)) { //neither h nor v match the last input, figure out prioritization from there 
            lastInput[0] = h;
            lastInput[1] = v;
            switch(directionFacing) {
                case "left":
                case "right":
                    switch(v) {
                        case 1:
                            myAnim.SetBool("Up", true);
                            myAnim.SetBool("Down", false);
                            myAnim.SetBool("Left", false);
                            myAnim.SetBool("Right", false);
                            directionFacing = "up";
                            break;
                        case -1:
                            myAnim.SetBool("Down", true);
                            myAnim.SetBool("Up", false);
                            myAnim.SetBool("Left", false);
                            myAnim.SetBool("Right", false);
                            directionFacing = "down";
                            break;
                        default:
                            break;
                    } 
                break;
                case "up":
                case "down":
                    switch(h) {
                        case 1:
                            myAnim.SetBool("Down", false);
                            myAnim.SetBool("Up", false);
                            myAnim.SetBool("Left", false);
                            myAnim.SetBool("Right", true);
                            directionFacing = "right";
                            break;
                        case -1:     //h = -1, left
                            myAnim.SetBool("Down", false);
                            myAnim.SetBool("Up", false);
                            myAnim.SetBool("Left", true);
                            myAnim.SetBool("Right", false);
                            directionFacing = "left";
                            break;
                        default:
                            break;    
                    }
                    break;
                default:
                    break;
                }
            return;
        }
    }

    public void animateSpell() {
        myAnim.SetBool("Spell", true);
    }

    public void castSpell() {
        GameObject.Find("AtkMenu5Options").GetComponent<CombatMenuController>().castSpell();
        enableWeaponWithoutHitbox();
    }

    private void stopAttack() {
        myAnim.SetBool("Attacking", false);
        myAnim.SetBool("Spell", false);
        disableWeapon();
        strafing = false;
    }

    private void enableWeapon() { //called with an animation event
        myWeapon.toggleWeapon(directionFacing);
    }

    private void disableWeapon() { //called with an animation event
        myWeapon.disableWeapon();
    }

    public void enableWeaponWithoutHitbox() { //called when casting a damaging spell. Animation framework should automatically disable the weapon sprite
        //myAnim.SetBool("Attacking", true);
        myWeapon.toggleWithoutHitbox(directionFacing);
        //Needs separate animations for spells because current framework is meant to automatically enable and disable hitbox and sprite
    }

    public void useItemAnimation() {
        myAnim.SetBool("Item", true);
        lockMovementNoUnlock();
    }

    public void stopItemAnimation() {
        myAnim.SetBool("Item", false);
        UnlockMovement();
    }

    public void lockMovement(float time) { //used for melee attacks and damaging spells
        inputsLocked = true;
        Invoke("UnlockMovement", time);
    }

    public void lockMovementNoUnlock() { //used for the item/healing animation, which has a built in call to unlock movement
        inputsLocked = true;
    }

    public void UnlockMovement() {
        inputsLocked = false;
    }

    public string getDirection() {
        return directionFacing;
    }

    public void setSprite(Sprite sprite) {
        mySpriteRenderer.sprite = sprite;
        mySpriteRenderer.enabled = false;
    }

    public void enableItemSprite() {
        mySpriteRenderer.enabled = true;
    }

    public void disableItemSprite() {
        mySpriteRenderer.enabled = false;
    }

    public void setAudio(AudioClip audioClip) {
        miscAudio = audioClip;
    }

    public void playAudio() {
        myAudioSource.PlayOneShot(miscAudio);
        //to pre-emptively prevent any potential glitches with the sound effects, immediately set misc audio to null after playing
        miscAudio = null;
    }

}
