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
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

                animator.SetBool("isMoving", success);
            } else {
                animator.SetBool("isMoving", false);
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
        canMove = false;
        //animator.SetTrigger("swordAttack");
    }

    public void LockMovement(){
        canMove = false;
    }

    public void UnlockMovement(){
        canMove = true;
    }


    /////////////////////////////////////////
    ///Logic for item interactions///////////
    /////////////////////////////////////////
    
public float interactionRadius = 2f; // Set the interaction radius (e.g., 2 units)
    private HighlightableItem closestItem = null;
    public LayerMask interactableLayer;  // Create a LayerMask for interactable items

    private void Update()
    {
        FindClosestItem();

        // Interact with the closest item if it's within range and the player presses the key
        if (closestItem != null && Input.GetKeyDown(KeyCode.Space))
        {
            closestItem.Interact();
        }
    }

    private void FindClosestItem()
    {
        // Use OverlapCircle to find all colliders within the interaction radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        float closestDistance = interactionRadius; // Set the max allowed interaction distance
        HighlightableItem newClosestItem = null;

        foreach (Collider2D collider in colliders)
        {
            HighlightableItem item = collider.GetComponent<HighlightableItem>();

            // Ensure the item has the HighlightableItem component
            if (item != null)
            {
                float distance = Vector2.Distance(transform.position, item.transform.position);

                // If this item is closer than the current closest item, consider it
                if (newClosestItem == null || distance < closestDistance)
                {
                    closestDistance = distance;
                    newClosestItem = item;
                }
            }
        }

        // If the closest item has changed, update the highlight states
        if (newClosestItem != closestItem)
        {
            // Unhighlight the previous closest item, if any
            if (closestItem != null)
            {
                closestItem.Unhighlight();
            }

            // Highlight the new closest item, if any
            if (newClosestItem != null)
            {
                newClosestItem.Highlight();
            }

            closestItem = newClosestItem;
        }

        // If no items are within the interaction radius, ensure no item remains highlighted
        if (newClosestItem == null && closestItem != null)
        {
            closestItem.Unhighlight();
            closestItem = null;
        }
    }

    // Optional: to visualize the interaction radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


}

