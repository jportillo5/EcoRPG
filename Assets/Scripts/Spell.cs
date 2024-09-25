using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Spell
{
    public string name;
    public string type; //categorizes the spell between attack, healing, or status effect
    public int cost; //MP cost of the spell
    public string target; //categorizes the spell based on who it targets
    public int power;
    public string description;
    public string effect; //extra effects potentially generated by a spell; effectChance determines its chance of happening.
    //The combat controller will determine what to do with the effect
    public float effectChance;

    //Getters
    public string getName() { return name; }
    public string getType() { return type;}
    public int getCost() { return cost; }
    public int getPower() { return power; }
    public string getDescription() { return description; }
    public string getEffect() { return effect; }
    public float getEffectChance() { return effectChance; }
}