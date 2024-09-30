using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    GameObject downWeapon;
    GameObject upWeapon;
    GameObject leftWeapon;
    GameObject rightWeapon;

    private bool damageLocked;
    // Start is called before the first frame update
    void Start()
    {
        downWeapon = GameObject.Find("WeaponDown");
        upWeapon = GameObject.Find("WeaponUp");
        leftWeapon = GameObject.Find("WeaponLeft");
        rightWeapon = GameObject.Find("WeaponRight");

        downWeapon.GetComponent<SpriteRenderer>().enabled = false;
        upWeapon.GetComponent<SpriteRenderer>().enabled = false;
        leftWeapon.GetComponent<SpriteRenderer>().enabled = false;
        rightWeapon.GetComponent<SpriteRenderer>().enabled = false;

        downWeapon.GetComponent<BoxCollider2D>().enabled = false;
        upWeapon.GetComponent<BoxCollider2D>().enabled = false;
        leftWeapon.GetComponent<BoxCollider2D>().enabled = false;
        rightWeapon.GetComponent<BoxCollider2D>().enabled = false;
        damageLocked = false;
    }

    public void toggleWeapon(string direction) {
        switch(direction) {
            case "down":
                downWeapon.GetComponent<SpriteRenderer>().enabled = true;
                downWeapon.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case "up":
                upWeapon.GetComponent<SpriteRenderer>().enabled = true;
                upWeapon.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case "left":
                leftWeapon.GetComponent<SpriteRenderer>().enabled = true;
                leftWeapon.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case "right":
                rightWeapon.GetComponent<SpriteRenderer>().enabled = true;
                rightWeapon.GetComponent<BoxCollider2D>().enabled = true;
                break;    
            default:
                break;    
        }
        Invoke("disableWeapon", 13/60f);
    }

    /*
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy") && !damageLocked) {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(1f);
            damageLocked = true;
        }
    }
    */
    
    private void disableWeapon() {
        downWeapon.GetComponent<SpriteRenderer>().enabled = false;
        upWeapon.GetComponent<SpriteRenderer>().enabled = false;
        leftWeapon.GetComponent<SpriteRenderer>().enabled = false;
        rightWeapon.GetComponent<SpriteRenderer>().enabled = false;

        downWeapon.GetComponent<BoxCollider2D>().enabled = false;
        upWeapon.GetComponent<BoxCollider2D>().enabled = false;
        leftWeapon.GetComponent<BoxCollider2D>().enabled = false;
        rightWeapon.GetComponent<BoxCollider2D>().enabled = false;

        damageLocked = false;
    }
}
