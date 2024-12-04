using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SpellTabController : MonoBehaviour
{
    Text spellDescription;
    Text spellTitle;
    List<Text> spellTexts;
    Image spellImage;
    List<Spell> spells;

    GameObject SubMenu1;

    GameObject SubMenu2;

    Text sm1Txt;
    Text sm2Txt;

    private bool inputsLocked;
    private int hoverY;
    private int hoverX;
    private int subHoverX;
    private string subMenu;

    PauseMenuController pmc;
    
    // Start is called before the first frame update
    void Start()
    {
        pmc = GameObject.Find("PauseMenu").GetComponent<PauseMenuController>();

        subMenu = "none";
        inputsLocked = true;
        hoverY = 1;
        hoverX = 2;

        spellDescription = GameObject.Find("SpellDescription").GetComponent<Text>();
        spellTitle = GameObject.Find("SpellName").GetComponent<Text>();
        spellImage = GameObject.Find("ItemDisplay").GetComponent<Image>();

        spellTexts = new List<Text>();
        spellTexts.Add(GameObject.Find("SpellText0").GetComponent<Text>());
        spellTexts.Add(GameObject.Find("SpellText1").GetComponent<Text>());
        spellTexts.Add(GameObject.Find("SpellText2").GetComponent<Text>());
        spellTexts.Add(GameObject.Find("SpellText3").GetComponent<Text>());

        SubMenu1 = GameObject.Find("SpellSubMenu1");
        SubMenu2 = GameObject.Find("SpellSubMenu2");
        SubMenu1.SetActive(false);
        SubMenu2.SetActive(false);
        
        sm1Txt = GameObject.Find("SSM1Text").GetComponent<Text>();
        sm2Txt = GameObject.Find("SSM2Text").GetComponent<Text>();

        spells = GameObject.Find("AtkMenu5Options").GetComponent<CombatMenuController>().getSpells();
    }

    void Update() {
        if(hoverY != 0) {
            hoverX = 2;
        }
        setTexts();
    }

    public void lockInputs() {
        inputsLocked = true;
    }

    public void unlockInputs() {
        inputsLocked = false;
    }

    void OnDPAD(InputValue input) {
        Vector2 navInput = input.Get<Vector2>();
            float h = navInput[0];
            float v = navInput[1];
        if(!inputsLocked) {

            if(v > 0) {
                hoverY--; //go up towards the top
            } else if (v < 0) {
                hoverY++; //go down towards the bottom
            }

            if(hoverY == 0) {
                if(h < 0) {
                    hoverX--;
                } else if(h > 0) {
                    hoverX++;
                }
            }

            if(hoverY > 5) {
                hoverY = 5;
            } else if(hoverY < 0) {
                hoverY = 0;
            }

            if(hoverX < 0) {
                hoverX = 3;
            } else if(hoverX > 3) {
                hoverX = 0;
            } //loop around

            setDescription();
        } else if(subMenu == "sub1") { //left/right
            if(h < 0) {
                subHoverX--;
            } else if(h > 0) {
                subHoverX++;
            }

            if(subHoverX > 2) {
                subHoverX = 2;
            } else if(subHoverX < 0) {
                subHoverX = 0;
            }

            setSubMenu1Text();
        } else if (subMenu == "sub2") {
            if(v > 0) {
                subHoverX--;
            } else if (v < 0) {
                subHoverX++;
            }

            if(subHoverX < 0) {
                subHoverX = 0;
            } else if(subHoverX > 3) {
                subHoverX = 3;
            }
            setSubMenu2Text();
        }
    }

    void OnConfirm() {
        if(!inputsLocked) {
            if(hoverY == 0) {
                inputsLocked = true;
                switch(hoverX) { //figure out which menu to go back to
                    case 0: //status page
                        hoverY = 1;
                        hoverX = 2;
                        pmc.enableTab("status");
                        break;
                    case 1: //spells page
                        hoverY = 1;
                        hoverX = 2;
                        pmc.enableTab("spells");
                        break;
                    case 2: //current tab
                        inputsLocked = false;
                        hoverY = 1;
                        break;
                    case 3: //options tab
                        hoverY = 1;
                        hoverX = 2;
                        pmc.enableTab("options");
                        break;            
                }
            } else {
                switch(subMenu) {
                    case "none":
                        inputsLocked = true;
                        subMenu = "sub1";
                        //open submenu 1
                        SubMenu1.SetActive(true);
                        setSubMenu1Text();
                        break;
                    case "sub1":
                        switch(subHoverX) {
                            case 0: //add to quickbinds
                                subMenu = "sub2";
                                //disable submenu 1
                                //SubMenu1.SetActive(false);
                                subHoverX = 0;
                                //enable submenu 2
                                SubMenu2.SetActive(true);
                                setSubMenu2Text();
                                break;
                            case 1: //same as return
                                subMenu = "none";
                                inputsLocked = false;
                                subHoverX = 0;
                                //disable this submenu
                                SubMenu1.SetActive(false);
                                break;
                        }
                        break;
                    case "sub2": //add quickbind
                        GameObject.Find("Inventory").GetComponent<QuickBinds>().setBind(subHoverX, spells[hoverY - 1].gameObject);
                        subMenu = "none";
                        inputsLocked = false;
                        subHoverX = 0;
                        //disable both submenus
                        SubMenu2.SetActive(false);
                        SubMenu1.SetActive(false);
                        break;    
                }
            }
        }
    }

    void OnReturn() {
        switch(subMenu) {
            case "none":
                if(hoverY == 0) {
                    hoverY = 1;
                    hoverX = 2;
                    pmc.enableTab("return");
                } else { //hoverY != 0
                    hoverY = 0;
                }
                break;
            case "sub1": //use, add to quickbinds, back. currently no need to mess with quick access items
                subMenu = "none";
                inputsLocked = false;
                subHoverX = 0;
                //disable this submenu
                SubMenu1.SetActive(false);
                break;
            case "sub2": //quickbinds slot 1, 2, 3, 4
                subMenu = "sub1";
                subHoverX = 0;
                //disable this submenu
                SubMenu2.SetActive(false);
                //enable submenu 1
                //SubMenu1.SetActive(true);
                break;
        }
    }

    private void setDescription() {
        if(hoverY != 0) {
            Spell spell = spells[hoverY - 1];

            spellTitle.text = spell.getName();
            spellDescription.text = spell.getDescription() + "\n\nPower - " + spell.getPower() 
            + "\nCost - " + spell.getCost() + " MP";
        } else if(hoverY == 0) {
            pmc.SetTabText(hoverX);
        }
    }

    private void setTexts() {
        for(int i = 0; i < spellTexts.Count; i++) {
            spellTexts[i].text = "";
            if(hoverY - 1 == i) {
                spellTexts[i].text += "> ";
            }
            spellTexts[i].text += spells[i].getName();
        }
    }

    private void setSubMenu1Text() {
        sm1Txt.text = "What would you like to do with this Spell?\n\n\n";
        switch(subHoverX) {
            case 0:
                sm1Txt.text += ">Add To Quickbinds\t\t\t\tGo Back";
                break;
            case 1:
                sm1Txt.text += "Add To Quickbinds\t\t\t\t>Go Back";
                break;     
        }
    }

    private void setSubMenu2Text() {
        GameObject[] binds = GameObject.Find("Inventory").GetComponent<QuickBinds>().getQuickBinds();
        sm2Txt.text = "Which slot would you like to put this in?\n\n\n";
        for(int i = 0; i < binds.Length; i++) {
            if(subHoverX == i) {
                sm2Txt.text += ">";
            }
            switch(binds[i].tag) {
                case "Spell":
                    sm2Txt.text += binds[i].GetComponent<Spell>().getName();
                    break;
                case "Item":
                    sm2Txt.text += binds[i].GetComponent<Item>().getName();
                    break;
                default:
                    sm2Txt.text += "Blank";
                    break;
            }
        }
    }
}

