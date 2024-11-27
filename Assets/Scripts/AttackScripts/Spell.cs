using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell : MonoBehaviour
{
    public string spellName;
    public string type; //categorizes the spell to determine its behavior
    public int cost; //MP cost of the spell
    public string description;
    public float healPower; //used for healing spells since I can't seem to get it through the attack object for some reason
    public float movementTimeout; //indicates how long to stop the player from moving. The animation framework manually handles this for healing spells
    public GameObject attack; //used to instantiate the object's hitbox and behaviors
    public AudioClip myClip;

    //Getters
    public string getName() { return spellName; }
    public string getType() { return type;}
    public int getCost() { return cost; }
    public string getDescription() { return description; }

    public AudioClip getAudio() { return myClip; }

    public void instantiateAttack(string direction, Transform t) {
        //ensure at least one MP is available. If the MP Bar's state is "normal", then the spell can be cast
        Player player = GameObject.Find("Sprout").GetComponent<Player>();
        PlayerController pc = GameObject.Find("Sprout").GetComponent<PlayerController>();
        if(GameObject.Find("MPBar").GetComponent<MPBarController>().getState() == "normal") {
            if(type == "heal") {
                //GameObject g = Instantiate(attack);
                GameObject.Find("Sprout").GetComponent<Player>().Heal(healPower);
                pc.useItemAnimation();
            } else {
                GameObject g = Instantiate(attack);
                g.transform.position = t.position;
                g.GetComponent<SpellAttack>().setSpellVelocity(direction);
                g.GetComponent<SpellAttack>().startCountdown();
                pc.enableWeaponWithoutHitbox();
                pc.lockMovement(movementTimeout);
            }
            //deplete MP
            GameObject.Find("Sprout").GetComponent<Player>().depleteMP(cost);    
        }

    }

    

}
