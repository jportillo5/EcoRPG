using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CombatController : MonoBehaviour //Placed in the combat scene
{
    private int expReward; //calculated by calling the getExpReward method from enemies' character sheets
    private GameObject[] enemies;
    private CharacterSheet playerSheet; //in the future, this will be a *little* more refined

    private string state; //represents the current step in the combat process

    Text pcHud;
    Text menuTxt;

    private List<string> options;
    private int hover;
    private int enemiesRemaining;

    private int enemyAttacking;

    //Variables to represent who the player is trying to attack and with what; in the future, this will likely be its own class for the sake of readability
    private string action;
    private Spell spellUsed;
    private int target; //index of the target

    //Variables pertaining to damage calculation
    private int power;
    private int attackerATK;
    private int defenderDEF;
    private int attackerLVL;
    
    // Start is called before the first frame update

    void Start()
    {
        //generate enemy list
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        pcHud = GameObject.Find("PC's health").GetComponent<Text>();
        menuTxt = GameObject.Find("Dialogue").GetComponent<Text>();
        playerSheet = GameObject.Find("CharacterBattle").GetComponent<CharacterSheet>();
        pcHud.text = "Sprout\nHP: " + playerSheet.getHP() + "\nMP: " + playerSheet.getMP() + "/" + playerSheet.getMaxMP();
        state = "pcAction";
        hover = 0;
        enemiesRemaining = enemies.Length;
        enemyAttacking = 0;
    }

    // Update is called once per frame
    void Update() //consideration - Should the attack happen immediately after deciding it or after choosing every party members' actions?
    { //This is probably an extremely primative method of programming menus, but it should suffice for demonstrating how this system MIGHT work
        pcHud.text = "Sprout\nHP: " + playerSheet.getHP() + "\nMP: " + playerSheet.getMP() + "/" + playerSheet.getMaxMP();
        
        determineMenu();
        float h = Input.GetAxis("Horizontal");
        if (h > 0 && hover < options.Count) {
            hover++;
        } else if (h < 0 && hover > 0) {
            hover--;
        }

        if(Input.GetButtonDown("Jump")) {
            determineNextState();
        }
        //Step 1 - choose PC's action.


        //Step 1a - if a spell, open spell list


        //Step 2 - Choose target.


        //Step 3 - Carry out attack. Repeat steps 1 - 3 for additional party members


        //Step 4 - Randomly select enemies' attacks and carry them out
    }

    //Current damage formula:((((((2 * Level) / 5) + 2) * P * (A/D)) / 50 ) + 2) * crit * other modifiers

    //Considerations: How to balance spread moves? lower damage? Higher cost?

    //certain field or status effects may need to applied here in some way
    private void determineMenu() { //menus and states will need to be made a lot more robust, but this should work for now
        switch(state) {
            case "pcAction" :
                actionMenu();
                break;
            case "pcSpell" :
                spellMenu();
                break;
            case "target" :
                targetMenu();
                break;
            case "attacking" : //actually carry out with the attack based on the variables set
                hover = 0;
                setDialogue();
                enemyAttacking = 0;
                break;
            case "enemyTurn" :
                enemyTurn();
                break;
            default : //waiting
                break;
        }
    }

    private void determineNextState() {
        switch(state) {
            case "pcAction" :
                if(hover == 0) {
                    //attack, next state is target
                    action = "melee";
                    state = "target";
                    hover = 0;
                } else { //hover == 1, since this is the only possible other option
                    //spell, next state is pcSpell
                    state = "pcSpell";
                    hover = 0;
                }
                break;
            case "pcSpell" :
                action = "spell";
                spellUsed = playerSheet.getSpell(hover);
                state = "target";
                hover = 0;
                break;
            case "target":
                target = hover;
                hover = 0;
                state = "attacking";
                break;
        }
    }

    private void actionMenu() {
        options = new List<string>();
        options.Add("Attack");
        options.Add("Spell");
        menuTxt.text = "What would you like to do?\n";
        for(int i = 0; i < options.Count; i++) {
            if(hover == i) {
                menuTxt.text += ">";
            }
            menuTxt.text += options[i] + "\t\t";
        }
    }

    private void spellMenu() {
        options = playerSheet.getSpellNames();
        menuTxt.text = "Which spell would you like to use?\n";
        for(int i = 0; i < options.Count; i++) {
            if(hover == i) {
                menuTxt.text += ">";
            }
            menuTxt.text += options[i] + "\t";
        }
        menuTxt.text += "\n" + playerSheet.getSpellCost(hover) + " MP\n" + playerSheet.getSpellDescription(hover);
    }

    private void targetMenu() {
        //The character sheets need to be updated to include the name of the enemy/party member, but for now I'm just gonna
        //do this the hard way
        options = new List<string>();
        if(action == "spell" && spellUsed.getType() == "heal") { //healing spell
            options.Add("Sprout");
        } else { //melee or attacking spell
            int enemyCount = getEnemyCount();
            for(int i = 0; i < enemyCount; i++) {
                options.Add("Slime");
            }
        }

        menuTxt.text = "Who would you like to target?\n";
        for(int i = 0; i < options.Count; i++) {
            if(hover == i) {
                menuTxt.text += ">";
            }
            menuTxt.text += options[i] + "\t";
        }
    }

    private void setDialogue() {
        menuTxt.text = "Sprout ";
        switch (action) {
            case "melee":
                menuTxt.text += "attacked with his " + playerSheet.weapon.getName(); //make getters for the weapon
                power = playerSheet.weapon.getPower();
                attackerATK = playerSheet.getAtk();
                defenderDEF = enemies[target].GetComponent<CharacterSheet>().getDef();
                attackerLVL = playerSheet.getLvl();
                break;
            case "spell":
                menuTxt.text += "used " + spellUsed.getName();
                playerSheet.mp -= spellUsed.getCost();
                //power = spellUsed.getPower();
                power = 10;
                attackerATK = playerSheet.getMatk();
                defenderDEF = enemies[target].GetComponent<CharacterSheet>().getDef();
                attackerLVL = playerSheet.getLvl();
                break;
            }
        if(action == "spell" && spellUsed.getType() == "heal") {
            //can only heal PC currently
            //playerSheet.hp += playerSheet.maxHP * (spellUsed.getPower() / 100);
        } else {
            enemies[target].GetComponent<CharacterSheet>().hp -= calculateDamage();
            if(enemies[target].GetComponent<CharacterSheet>().hp <= 0) {
                enemies[target].SetActive(false);
                enemiesRemaining--;
            }
        }
        state = "waiting";
        Invoke("setNextState", 2f);
    }

    private int getEnemyCount() {
        int enemyCount = 0;
        for(int i = 0; i < enemies.Length; i++) {
            if(enemies[i].GetComponent<CharacterSheet>().getHP() > 0) {
                enemyCount++;
            }
        }
        return enemyCount;
    }

    private int calculateDamage() {
        int damage = ((((((2 * attackerLVL) / 5) + 2) * power * (attackerATK/defenderDEF)) / 50 ) + 2);
        return damage;
    }

    private void setNextState() {
        state = "enemyTurn";
    }

    private void enemyTurn() {
        int index = enemyAttacking; // too lazy to rewrite this
        if(enemies[index].GetComponent<CharacterSheet>().getHP() > 0) {
            System.Random rnd = new System.Random();
            int move = rnd.Next(0, 1);
            attackerLVL = enemies[index].GetComponent<CharacterSheet>().getLvl();
            defenderDEF = playerSheet.getDef();
            if(move == 0) { //melee
                menuTxt.text = "The slime lunged at Sprout"; //would be changed in the future for better flavor text
                attackerATK = enemies[index].GetComponent<CharacterSheet>().getAtk();
                power = enemies[index].GetComponent<CharacterSheet>().weapon.getPower();
            } else { //spell. This sequence would be changed to randomly choose a spell, or some other more complex selection algorithm
                menuTxt.text = "The slime spat at Sprout"; //would be changed in the future for better flavor text
                attackerATK = enemies[index].GetComponent<CharacterSheet>().getMatk();
                //power = enemies[index].GetComponent<CharacterSheet>().spells[0].getPower();
                power = 10;
            }

            playerSheet.hp -= calculateDamage();
            state = "waiting";
            if(playerSheet.hp <= 0) {
                Invoke("gameOver", 1f);
            } else {
                Invoke("delayNextEnemy", 2f);
            }
        } else {
            enemyAttacking++;
            enemyTurn();
        }
    }

    private void delayNextEnemy() {
        enemyAttacking++;
        if(enemyAttacking < enemies.Length) {
            enemyTurn();
        } else {
            state = "pcAction";
        }
    }

    private void gameOver() {
        GameObject.Find("CharacterBattle").SetActive(false);
        menuTxt.text = "Sprout has taken fatal damage. Game Over!";
    }

}
