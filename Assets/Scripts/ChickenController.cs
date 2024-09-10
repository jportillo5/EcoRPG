using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChickenController : MonoBehaviour
{
public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    //Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

   
    private void FixedUpdate()
    // If movement input is not 0, try to move
    {
        if(canMove){
            if(movementInput != Vector2.zero){
                bool success = TryMove(movementInput);

                if(!success && movementInput.x > 0) {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if(!success && movementInput.y > 0){
                        success = TryMove(new Vector2(0, movementInput.y));
                    }

                //animator.SetBool("isMoving", success);
            } else {
                //animator.SetBool("isMoving", false);
            }

            //Set direction od sprite to movement direction
            if(movementInput.x < 0){
            spriteRenderer.flipX = true;
            } else if(movementInput.x > 0){
            spriteRenderer.flipX = false;
            }
        }

    }

    private bool TryMove(Vector2 direction) {
        int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0){
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
            } else {
                return false;
            }
    }

    

    void OnMove(InputValue movementValue) {
        //canMove = true;
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
        //canMove = false;
        //animator.SetTrigger("swordAttack");
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }

}

