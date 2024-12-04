using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
//using UnityEditor.Animations;
using UnityEngine.InputSystem;
//using UnityEditor.Callbacks;
using UnityEngine.Android;

public class CombatMenuController : MonoBehaviour
{
    PlayerController player;
    List<Text> optionTexts = new List<Text>();
    List<string> options = new List<string>();

    public List<Spell> spellList;
    public Sprite defaultHealItemSprite;
    private Sprite itemSprite;
    //public List<GameObject> quickBinds;

    private int hover;
    private string menu;
    private bool quickbindsOn;
    private bool lockNav;
    private QuickBinds quickBinds;
    private InventoryManager invManager;
    
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
        lockNav = false; //prevents navigation of the menu when casting a spell before it's successfully cast
        quickBinds = GameObject.Find("Inventory").GetComponent<QuickBinds>();
        itemSprite = defaultHealItemSprite;
        invManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
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
                    if(GameObject.Find("MPBar").GetComponent<MPBarController>().getState() == "normal") {
                        lockNav = true;
                        //check to see if it's a damaging or healing spell
                        if(spellList[hover].getType() == "heal") {
                            itemSprite = defaultHealItemSprite;
                            player.setSprite(itemSprite);
                            player.useItemAnimation();
                            castSpell();
                        } else {
                            player.animateSpell();
                        }
                    }
                    break;
                case "items":
                    lockNav = true;
                    player.setSprite(invManager.getItemSprite(hover));
                    //animation will be triggered from the item object itself
                    invManager.useItem(hover);
                    quickbindsOn = false;
                    OnReturn(); //prevent accidental double usage of items.
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
                    options.Add(binds[i].GetComponent<Item>().getName() + " x" + binds[i].GetComponent<Item>().getCount());
                    break;
                default:
                    options.Add("Blank");
                    break;
            }
        }
    }

    private void getItemOptions() {
        options.Clear();
        List<Item> items = invManager.getItemObjects();
        foreach (Item item in items) {
            options.Add(item.getName() + " x" + item.getCount());
        }
        options.Add("");
        options.Add("");
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

    public void castSpell() { //called by an animation event to instantiate the spell
        spellList[hover].instantiateAttack(getPlayerDirection(), player.GetComponentInParent<Transform>());
    }

    public Sprite getItemSprite() {
        return itemSprite;
    }

    public List<Spell> getSpells() {
        return spellList;
    }
}
