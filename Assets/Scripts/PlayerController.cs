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
    SpriteRenderer mySpriteRenderer;
    Animator myAnim;
    Vector2 movementInput;
    Rigidbody2D myBod;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    WeaponController myWeapon;

    //Components related to other game objects probably
    
    //Private properties
    private float[] lastInput;
    private string directionFacing;

    //public properties
    public float moveSpeed;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        lastInput = new float[] {0, 0}; //no input
        myBod = GetComponent<Rigidbody2D>();
        directionFacing = "down";
        myWeapon = GetComponentInChildren<WeaponController>();
    }

    private void FixedUpdate() { //if regular update isn't necessary, move stuff from update to here ig?
        //If movement input is not 0, try to move, otherwise idle
        if(movementInput != Vector2.zero) {
            int count = myBod.Cast(
                movementInput,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
                );
            if(count == 0) {
                myBod.MovePosition(myBod.position + movementInput * moveSpeed * Time.fixedDeltaTime);
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

    void OnFire() { //hitboxes will be controlled by a sort of weapon controller,
    //which enables and disables the sprite renderer and has a collider to cross-check with enemy colliders
        myAnim.SetBool("Attacking", true);
        Invoke("enableWeapon", 13/60f);
        Invoke("stopAttack", 0.2f);
    }

    private void determineDirectionOfMovement(float h, float v) { //more like direction of animation
        //last input same as new input.
        if((lastInput[0] == h) && (lastInput[1] == v)) { 
            //change direction facing
            myAnim.SetBool("Moving", true);
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

    private void stopAttack() {
        myAnim.SetBool("Attacking", false);
    }

    private void enableWeapon() {
        myWeapon.toggleWeapon(directionFacing);
    }

}
