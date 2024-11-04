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
    private bool quickbindsOn;

    private QuickBinds quickBinds;
    
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
        quickbindsOn = false;
        quickBinds = GameObject.Find("Inventory").GetComponent<QuickBinds>();
    }

    // Update is called once per frame
    void Update()
    {     

        if(hover <=0) {
            hover = 0;
        } else if (hover >=4) {
            hover = 4;
        } //this block should be changed to be able to loop around to the menu and display different options

        SetOptions();
    }

    void OnDPAD(InputValue input) {
        if(quickbindsOn) {
            return;
        }
        Vector2 navInput = input.Get<Vector2>();

        float v = navInput[1]; //y value

        if(v > 0) {
            hover--;
        } else if(v < 0) {
            hover++;
        }

        if(hover <=0) {
            hover = 0;
        } else if (hover >=4) {
            hover = 4;
        } //change this block to loop around the options
    }

    void OnReturn() {
        if(quickbindsOn) {
            quickBinds.Use(1);
            getQuickBindOptions();
        } else {
            hover = 0;
            menu = "default";
            getDefaultOptions();
        }
    }

    void OnFire() {
        if(quickbindsOn) {
            quickBinds.Use(0);
        } else {
            switch(menu) {
                case "default":
                    switch(hover) {
                        case 0:
                            player.attack();
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
                    //perform animations
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
    }

    void OnLBump() {
        if(!quickbindsOn) {
            return;
        }
        quickBinds.Use(2);
    }

    void OnRBump() {
        if(!quickbindsOn) {
            return;
        }
        quickBinds.Use(3);
    }

    void OnQuickBind(InputValue qbValue) {
        if(qbValue.isPressed) {
            quickbindsOn = true;
            getQuickBindOptions();
        } else {
            quickbindsOn = false;
            menu = "default";
            getDefaultOptions();
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

    private void getQuickBindOptions() {
        options.Clear();
        GameObject[] binds = quickBinds.getQuickBinds();
        for(int i = 0; i < binds.Length; i++) {
            switch(binds[i].tag) {
                case "Spell":
                    options.Add(binds[i].GetComponent<Spell>().getName());
                    break;
                case "Item":
                    options.Add("Implement Items");
                    break;
                default:
                    options.Add("Blank");
                    break;        
            }
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
            if(hover == i && !quickbindsOn) {
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
