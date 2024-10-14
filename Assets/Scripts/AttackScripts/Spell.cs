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

    public GameObject attack; //used to instantiate the object's hitbox and behaviors

    //Getters
    public string getName() { return spellName; }
    public string getType() { return type;}
    public int getCost() { return cost; }
    public string getDescription() { return description; }

    public void instantiateAttack(string direction, Transform t) {
        GameObject g = Instantiate(attack);
        g.transform.position = t.position;
        g.GetComponent<SpellAttack>().setSpellVelocity(direction);
        if(type == "exploding") {
            g.GetComponent<SpellAttack>().startCountdown();
        }
    }


}
