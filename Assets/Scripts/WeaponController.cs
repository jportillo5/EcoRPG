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
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy") {
            //damage the enemy
        }
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
    
    private void disableWeapon() {
        downWeapon.GetComponent<SpriteRenderer>().enabled = false;
        upWeapon.GetComponent<SpriteRenderer>().enabled = false;
        leftWeapon.GetComponent<SpriteRenderer>().enabled = false;
        rightWeapon.GetComponent<SpriteRenderer>().enabled = false;

        downWeapon.GetComponent<BoxCollider2D>().enabled = false;
        upWeapon.GetComponent<BoxCollider2D>().enabled = false;
        leftWeapon.GetComponent<BoxCollider2D>().enabled = false;
        rightWeapon.GetComponent<BoxCollider2D>().enabled = false;
    }
}
