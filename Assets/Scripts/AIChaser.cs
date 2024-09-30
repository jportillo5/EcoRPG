using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaser : MonoBehaviour
{

    public GameObject player;
    public float speed;
    public float distanceBetween;
    private float distance;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if(distance < distanceBetween){
            animator.SetBool("isMoving", true);
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else {
            animator.SetBool("isMoving", false);
        }

        
    }
}
