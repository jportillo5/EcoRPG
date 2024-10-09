using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellAttack : MonoBehaviour
{
    public float power;
    public float projectileSize;

    public float speed;
    //public float type;
    public float explosionSize; //hopefully this works? set to 0 for non-exploding projectiles
    private Vector3 movement;
    
    void Start() {
        movement = new Vector3(0, 1, 0);
    }

    void Update() {
        transform.position += movement * speed * Time.deltaTime;
    }
    
    
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(power);
            explode();
        }
    }

    public void explode() {
        //handle hitbox change for explosion
        transform.localScale = Vector3.one * explosionSize;
        //wait to destroy self
        Invoke("destroySelf", 2);
    }

    private void destroySelf() {
        Destroy(gameObject);
    }

    
    public void setSpellVelocity(string direction) {
        switch(direction) {
            case "up":
                movement = new Vector3(0, 1, 0);
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case "down":
                movement = new Vector3(0, -1, 0);
                transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case "left":
                movement = new Vector3(-1, 0, 0);
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case "right":
                movement = new Vector3(1, 0, 0);
                transform.eulerAngles = new Vector3(0, 0, 270);
                break;            
        }
    }

    public void startCountdown() {
        Invoke("explode", 3);
    }
}
