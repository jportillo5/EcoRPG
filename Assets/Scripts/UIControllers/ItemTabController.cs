using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemTabController : MonoBehaviour
{
    public float stickSens;
    public List<Color> panelColors;
    
    Text itemDescription;
    Text itemTitle;
    List<Text> itemTexts;
    List<Image> itemPanels;
    Image itemImage;
    List<Item> items;

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
        subHoverX = 0; //misleading name, set to work with x or y depending on the submenu

        itemDescription = GameObject.Find("ItemDescription").GetComponent<Text>();
        itemTitle = GameObject.Find("ItemName").GetComponent<Text>();
        itemImage = GameObject.Find("ItemDisplay").GetComponent<Image>();

        itemTexts = new List<Text>();
        itemTexts.Add(GameObject.Find("ItemText0").GetComponent<Text>());
        itemTexts.Add(GameObject.Find("ItemText1").GetComponent<Text>());
        itemTexts.Add(GameObject.Find("ItemText2").GetComponent<Text>());
        itemTexts.Add(GameObject.Find("ItemText3").GetComponent<Text>());

        itemPanels = new List<Image>();
        itemPanels.Add(GameObject.Find("IPanel1").GetComponent<Image>());
        itemPanels.Add(GameObject.Find("IPanel2").GetComponent<Image>());
        itemPanels.Add(GameObject.Find("IPanel3").GetComponent<Image>());
        itemPanels.Add(GameObject.Find("IPanel4").GetComponent<Image>());

        SubMenu1 = GameObject.Find("ItemSubMenu1");
        SubMenu2 = GameObject.Find("ItemSubMenu2");
        
        sm1Txt = GameObject.Find("ISM1Text").GetComponent<Text>();
        sm2Txt = GameObject.Find("ISM2Text").GetComponent<Text>();

        items = GameObject.Find("Inventory").GetComponent<InventoryManager>().getItemObjects();

        SubMenu1.SetActive(false);
        SubMenu2.SetActive(false);
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

    void OnUIMove(InputValue input) {
        Vector2 navInput = input.Get<Vector2>();
        float h = navInput[0];
        float v = navInput[1];
        if(!inputsLocked) {
            if(v > stickSens) {
                hoverY--; //go up towards the top
            } else if (v < -stickSens) {
                hoverY++; //go down towards the bottom
            }

            if(hoverY > 2) { //currently only 3 options, no need to go past this
                hoverY = 2;
            } else if(hoverY < 0) {
                hoverY = 0;
            } else if(Math.Abs(v) > stickSens) { //ensuring it actually attempted to move to a different option
                pmc.playNavClip();
            }

            if(hoverY == 0) {
                if(h < -stickSens) {
                    hoverX--;
                } else if(h > stickSens) {
                    hoverX++;
                }
                pmc.playNavClip();
            }

            if(hoverX < 0) {
                hoverX = 3;
            } else if(hoverX > 3) {
                hoverX = 0;
            } //loop around

            setDescription();
        } else if(subMenu == "sub1") { //left/right
            if(h < -stickSens) {
                subHoverX--;
            } else if(h > stickSens) {
                subHoverX++;
            }

            if(subHoverX > 2) {
                subHoverX = 2;
            } else if(subHoverX < 0) {
                subHoverX = 0;
            } else if(Math.Abs(h) > stickSens) {
                pmc.playNavClip();
            }

            setSubMenu1Text();
        } else if (subMenu == "sub2") {
            if(v > stickSens) {
                subHoverX--;
            } else if (v < -stickSens) {
                subHoverX++;
            }

            if(subHoverX < 0) {
                subHoverX = 0;
            } else if(subHoverX > 3) {
                subHoverX = 3;
            } else if(Math.Abs(v) > stickSens) {
                pmc.playNavClip();
            }
            setSubMenu2Text();
        }
    }

    void OnUIConfirm() {
        if(!inputsLocked) {
            pmc.playConfirmClip();
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
            } else { //hovering over an item, not in a submenu
                inputsLocked = true;
                subMenu = "sub1";
                //open submenu 1
                SubMenu1.SetActive(true);
                setSubMenu1Text();
            }
        } else { //inputs locked, either tab is disabled or in a submenu
            switch(subMenu) {
                case "sub1":
                    switch(subHoverX) {
                        case 0:
                            pmc.playConfirmClip();
                            //use the item in hoverY - 1
                            items[hoverY - 1].useItem();
                            setTexts();
                            setDescription();
                            subMenu = "none";
                            //disable submenu 1
                            SubMenu1.SetActive(false);
                            inputsLocked = false;
                            subHoverX = 0;
                            break;
                        case 1: //add to quickbinds
                            pmc.playConfirmClip();
                            subMenu = "sub2";
                            //disable submenu 1
                            //SubMenu1.SetActive(false);
                            subHoverX = 0;
                            //enable submenu 2
                            SubMenu2.SetActive(true);
                            setSubMenu2Text();
                            break;
                        case 2: //same as return
                            pmc.playConfirmClip();
                            subMenu = "none";
                            inputsLocked = false;
                            subHoverX = 0;
                            //disable this submenu
                            SubMenu1.SetActive(false);
                            break;
                    }
                break;
            case "sub2": //add quickbind
                pmc.playConfirmClip();
                GameObject.Find("Inventory").GetComponent<QuickBinds>().setBind(subHoverX, items[hoverY - 1].gameObject);
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

    void OnUIReturn() {
        switch(subMenu) {
            case "none":
                if(hoverY == 0) {
                    hoverY = 1;
                    hoverX = 2;
                    inputsLocked = true;
                    pmc.enableTab("return");
                } else { //hoverY != 0
                    hoverY = 0;
                }
                break;
            case "sub1": //use, add to quickbinds, back. currently no need to mess with quick access items
                pmc.playReturnClip();
                subMenu = "none";
                inputsLocked = false;
                subHoverX = 0;
                //disable this submenu
                SubMenu1.SetActive(false);
                break;
            case "sub2": //quickbinds slot 1, 2, 3, 4
                pmc.playReturnClip();
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
            Item item = items[hoverY - 1];

            itemTitle.text = item.getName();
            itemDescription.text = item.getDescription() + "\n\n\nCount - " + item.getCount();
            itemImage.sprite = item.getIcon();
        } else if(hoverY == 0) {//handle tabs
            pmc.SetTabText(hoverX);
        }
    }

    private void setTexts() {
        for(int i = 0; i < itemTexts.Count; i++) {
            itemTexts[i].text = "";
            if(hoverY - 1 == i) {
                itemPanels[i].color = panelColors[1];
            } else {
                itemPanels[i].color = panelColors[0];
            }
        }
        //in the future, replace this with a more robust method of
        //automatically configuring what the item panels display
        itemTexts[0].text += "Fruit Pitcher";
        itemTexts[1].text += "Honey Pot";
        itemTexts[2].text += "";
        itemTexts[3].text += "";

    }

    private void setSubMenu1Text() {
        sm1Txt.text = "What would you like to do with this item?\n\n\n";
        switch(subHoverX) {
            case 0:
                sm1Txt.text += ">Use\t\t\t\tAdd To Quickbinds\t\t\t\tBack";
                break;
            case 1:
                sm1Txt.text += "Use\t\t\t\t>Add To Quickbinds\t\t\t\tBack";
                break;
            case 2:
                sm1Txt.text += "Use\t\t\t\tAdd To Quickbinds\t\t\t\t>Back";
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
            sm2Txt.text += "\n\n\n";
        }
    }
}
