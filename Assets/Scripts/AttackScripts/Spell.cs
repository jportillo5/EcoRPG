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
    public GameObject attack; //used to instantiate the object's hitbox and behaviors

    private MPBarController mpBar;
    private Player player;
    /*
    void Start() {
        mpBar = GameObject.Find("MPBar").GetComponent<MPBarController>();
        player = GameObject.Find("Sprout").GetComponent<Player>();
    }
    */

    //Getters
    public string getName() { return spellName; }
    public string getType() { return type;}
    public int getCost() { return cost; }
    public string getDescription() { return description; }

    public void instantiateAttack(string direction, Transform t) {
        //ensure at least one MP is available. If the MP Bar's state is "normal", then the spell can be cast
        if(GameObject.Find("MPBar").GetComponent<MPBarController>().getState() == "normal") {
            if(type == "heal") {
                //GameObject g = Instantiate(attack);
                GameObject.Find("Sprout").GetComponent<Player>().Heal(healPower);
            } else {
                GameObject g = Instantiate(attack);
                g.transform.position = t.position;
                g.GetComponent<SpellAttack>().setSpellVelocity(direction);
                if(type == "exploding" || type == "beam") {
                    g.GetComponent<SpellAttack>().startCountdown();
                }
            }
            //deplete MP
            GameObject.Find("Sprout").GetComponent<Player>().depleteMP(cost);    
        }

    }


}
