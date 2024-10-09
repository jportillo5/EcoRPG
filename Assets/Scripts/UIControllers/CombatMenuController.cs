using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEditor.Callbacks;
using UnityEngine.Android;

public class CombatMenuController : MonoBehaviour
{
    PlayerController player;
    List<Text> optionTexts = new List<Text>();
    List<string> options = new List<string>();

    public List<Spell> spellList;
    //public List<GameObject> quickBinds;

    private int hover;
    private string menu;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Sprout").GetComponent<PlayerController>();
        hover = 0;
        menu = "default";
        optionTexts.Add(GameObject.Find("AtkTxt").GetComponent<Text>());
        optionTexts.Add(GameObject.Find("SpellTxt").GetComponent<Text>());
        optionTexts.Add(GameObject.Find("ItemTxt").GetComponent<Text>());
        optionTexts.Add(GameObject.Find("Opt4Txt").GetComponent<Text>());
        getDefaultOptions();
    }

    // Update is called once per frame
    void Update()
    {     
        if(Input.GetButtonDown("Fire2")) { //this block is a hotfix to test functionality of spells
            spellList[0].instantiateAttack(getPlayerDirection(), player.GetComponentInParent<Transform>());
        }
        
        //Q goes up the list, E goes down the list, R returns to the default menu.
        //On controller, L goes up, R goes down, and B goes back
        /*
        if(Input.GetKeyDown("Q")) {
            hover--;
        } else if(Input.GetKeyDown("E")) {
            hover++;
        } else if(Input.GetKeyDown("R")) {
            hover = 0;
            menu = "default";
            getDefaultOptions();
        }
        */

        if(hover <=0) {
            hover = 0;
        } else if (hover >=4) {
            hover = 4;
        } //this block should be changed to be able to loop around to the menu and display different options

        //On left click, move to next menu or enact appropriate option

        SetOptions();
    }

    void OnFire() {
        switch(menu) {
            case "default":
                switch(hover) {
                    case 0:
                        //attack with sword
                        break;
                    case 1:
                        hover = 0;
                        menu = "spells";
                        getSpellOptions();
                        break;
                    case 2:
                        hover = 0;
                        menu = "items";  
                        getItemOptions();
                        break;
                    default:
                        hover = 0;
                        break;    
                }
                break;
            case "spells":
                spellList[hover].instantiateAttack(getPlayerDirection(), player.GetComponentInParent<Transform>());
                break;
            case "items":
                switch(hover) { //replace block with call to item's function
                    case 0:
                        //heal player
                        break;
                    case 1:
                        //restore MP
                        break;
                    case 2:
                        //fully restore HP and MP
                        break;
                    case 3:
                        //but nothing happened
                        break;  
                }
                break;    
        }
    }

    private void getDefaultOptions() {
        options.Clear();
        options.Add("Attack");
        options.Add("Spell");
        options.Add("Item");
        options.Add("");
    }

    private void getSpellOptions() {
        options.Clear();
        //In the future, pull spells from an inventory. For now, just manually generate them
        for(int i = 0; i < spellList.Count; i++) {
            options.Add(spellList[i].getName());
        }
    }

    private void getItemOptions() {
        options.Clear();
        //in the future, pull items from an inventory. For now, manually generate some
        options.Add("Potion"); //heals two hearts each(?)
        options.Add("Honey"); //restores all MP
        options.Add("Elixir of Life"); //restores all HP and MP
        options.Add("uh....."); //Do nothing for now, eventually implement being able to have less than 4 options
    }

    private void SetOptions() {
        clearOptions();
        for(int i = 0; i < optionTexts.Count; i++) {
            if(hover == i) {
                optionTexts[i].text += "> ";
            }
            optionTexts[i].text += options[i];
        }
    }

    private void clearOptions() {
        for(int i = 0; i < optionTexts.Count; i++) {
            optionTexts[i].text = "";
        }
    }

    private string getPlayerDirection() {
        return player.getDirection();
    }
}
