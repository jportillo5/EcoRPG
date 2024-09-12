using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon
{
    
    public string name;
    public int power;
    public string description;

    //getters
    public string getName() { return name; }
    public int getPower() { return power; }
    public string getDescription() { return description; }
}
