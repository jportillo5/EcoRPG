using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 2f;
    Rigidbody2D myRigidbody;
    Vector2 change;
    float h;
    float v;




    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // change = Vector3.zero;
        // change.x = Input.GetAxis("Horizontal");
        // change.y = Input.GetAxis("Vertical");

        // if (change != Vector2.zero)
        // {
        //     MoveCharacter();
        // }

        Debug.Log(change);

        change = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MoveCharacter();
    }


    void MoveCharacter()
    {
    //    myRigidbody.MovePosition((Vector2)transform.position + change * speed * Time.deltaTime);
        //myRigidbody.velocity = new UnityEngine.Vector2( h * 4f, myRigidbody.velocity.y);
        myRigidbody.velocity = change * speed;
    }
}
