using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Animations;

public class PlayerController : MonoBehaviour
{
    //Components Connected to this game object
    SpriteRenderer mySpriteRenderer;
    Animator myAnim;
    
    //Private properties
    private string directionFacing;
    private float[] lastInput;
    private Boolean inMotion;
    
    //Public Properties
    public Sprite leftIdle;
    public Sprite rightIdle;
    public Sprite upIdle;
    public Sprite downIdle;
    
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        inMotion = false;
        directionFacing = "down";
        mySpriteRenderer.sprite = downIdle;
        lastInput = new float[] {0, 0}; //no input
    }

    // Update is called once per frame
    void Update()
    {
        //left -1h, right +1h; up +1v, down -1v
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //If going from idle to in motion, prioritize vertical over horizontal inputs if both are made at the same time.
        //If already in motion, change in direction takes priority
        
        //if in motion, finish movement to next tile before switching directions

        //moving one tile should take 20 frames with current animation speed, can be adjusted

        //walking uses the animator's "Moving" boolean parameter.
        //The direction the player walks in corresponds to the "Direction" integer parameter
        //0 for up, 1 for left, 2 for down, 3 for right
        if(inMotion) {
            //wait for motion to next tile to finish
        } else { //!inMotion
            if(h == 0 && v == 0) {
                myAnim.SetBool("Moving", false);
                lastInput[0] = 0;
                lastInput[1] = 0;
                //updateIdleSprite();
                //don't apply motion
            } else { //continue moving, determine direction
                string direction = determineDirectionOfMovement(h, v);
                updateDirectionFacing(direction);
                updateIdleSprite();

                inMotion = true;
                myAnim.SetBool("Moving", true);
                //invoke endMotion in 20 frames
                Invoke("endMotion", .333f); //doess not appear to work as intended
            }
        }
    }

    //updates the sprite used in the player's idle state
    private void updateIdleSprite() {
        if(directionFacing == "left") {
            mySpriteRenderer.sprite = leftIdle;
        } else if (directionFacing == "right") {
            mySpriteRenderer.sprite = rightIdle;
        } else if (directionFacing == "up") {
            mySpriteRenderer.sprite = upIdle;
        } else {
            mySpriteRenderer.sprite = downIdle;
        }
    }

    //I might be overthinking some of this
    private void updateDirectionFacing(string direction) {
        directionFacing = direction;
    }

    private string determineDirectionOfMovement(float h, float v) { //there appear to be problems with this method, will be investigated later
        //last input same as new input. Continue moving in same direction
        if((lastInput[0] == h) && (lastInput[1] == v)) { 
            //apply motion based on direction facing

            //no need to change animation state
            return directionFacing;
        }
        
        //last h == new h, last v != new v; prioritize vertical input
        else if((lastInput[0] == h) && (lastInput[1] != v)) {
            lastInput[1] = v;
            //change animation state and apply motion
            if(v == 1) { //up
                myAnim.SetInteger("Direction", 0);

                return "up";
            } else { //v == -1, down
                myAnim.SetInteger("Direction", 2);

                return "down";
            }
        }

        //last h != new h, last v == new v; prioritize horizontal input
        else if((lastInput[0] != h) && (lastInput[1] == v)) {
            lastInput[0] = h;
            //change animaion state and apply motion
            if(h == 1) { //right
                myAnim.SetInteger("Direction", 3);

                return "right";
            } else { //h = -1, left
                myAnim.SetInteger("Direction", 1);

                return "left";
            }
        }

        else {// neither horizontal nor vertical match the last input. prioritize vertical input
            lastInput[0] = h;
            lastInput[1] = v;

            //change animation state and apply motion
            if(v == 1) { //up
                myAnim.SetInteger("Direction", 0);

                return "up";
            } else { //v == -1, down
                myAnim.SetInteger("Direction", 2);

                return "down";
            }
        }
    }

    private void endMotion() {
        inMotion = false;
    }
}
