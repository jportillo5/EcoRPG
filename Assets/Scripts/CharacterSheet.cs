using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable] //not sure if we need this
public class CharacterSheet : MonoBehaviour //Represents the character's stats, spell list, etc. Used by both players and enemies
{
    //Character's stats
    
    public int hp; //health points //currently missing max HP
    public int mp; //mana points, used to cast spells. Change to be private later

    public int maxHP;
    public int maxMP;
    public int atk; //physical strength
    public int matk; //magical strength
    public int def; //will cover both physical and magic attacks
    public int spd; //speed, determines if the player's side or enemy side goes first
    public int exp; //experience points
    public int lvl; //current level
    public int expForNextLevel; //experience points that will trigger a level up; upon a level up, calculate the next milestone
    public int expReward; //used by enemies to reward experience points to the player's team
    public int cashReward; //used by enemies to reward funds to the player to purchase items, spells, etc.
    
    //need some representation of character's spells and melee weapon
    public List<Spell> spells;
    public Weapon weapon;

    //Getters
    public int getHP() { return hp; }
    public int getMP() { return mp; }
    public int getMaxMP() { return maxMP; }
    public int getAtk() { return atk; }
    public int getMatk() { return matk; }
    public int getDef() { return def; }
    public int getSpd() { return spd; }
    public int getExp() { return exp; }
    public int getLvl() { return lvl; }
    public int getExpForNextLevel() { return expForNextLevel; }
    public int getExpReward() { return expReward; }

    public List<string> getSpellNames() {
        List<string> spellNames = new List<string>();
        foreach(Spell s in spells) {
            spellNames.Add(s.getName());
        }
        return spellNames;
    }

    //get the necessary datat from the spell

    public string getSpellDescription(int i) {
        return spells[i].getDescription();
    }

    public int getSpellCost(int i) {
        return spells[i].getCost();
    }

    public Spell getSpell(int i) {
        return spells[i];
    }
}
