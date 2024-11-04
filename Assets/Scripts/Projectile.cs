using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 direction = new Vector2(1,0);
    public float speed;
    public Vector2 velocity;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        velocity = direction * speed;
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;

        pos += velocity * Time.fixedDeltaTime;

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null) {
                player.TakeDamage(20f);  // Deal damage
                //Debug.Log("Player took damage!");
                Destroy(gameObject);  // Destroy projectile on impact
            }
        }
        // else if (collision.CompareTag("Wall"))  // Destroy if it hits a wall
        // {
        //     Destroy(gameObject);
        // }
    }
}
