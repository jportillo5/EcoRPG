using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatusTabController : MonoBehaviour
{
    public float stickSens;

    public List<Color> panelColors;
    //0 - default
    //1 - hover
    List<GameObject> menuPanels;

    List<GameObject> panelDisplays;
    /*
        [hoverY, hoverX]
        [0, 0], [0, 1]
        [1, 0], [1, 1]
    */
    List<Text> panelTexts;

    List<Spell> spells;
    List<Item> items;

    Text hpTxt;
    Text mpTxt;

    private int hoverX;
    private int hoverY;
    private bool inputsLocked;

    PauseMenuController pmc;
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        pmc = GameObject.Find("PauseMenu").GetComponent<PauseMenuController>();

        panelDisplays = new List<GameObject>();
        panelDisplays.Add(GameObject.Find("ObjectiveDisplay"));
        panelDisplays.Add(GameObject.Find("QuickbindsDisplay"));
        panelDisplays.Add(GameObject.Find("PreppedSpellsDisplay"));
        panelDisplays.Add(GameObject.Find("QAIDisplay"));

        panelTexts = new List<Text>();
        panelTexts.Add(GameObject.Find("QBDText").GetComponent<Text>());
        panelTexts.Add(GameObject.Find("PSDText").GetComponent<Text>());
        panelTexts.Add(GameObject.Find("QAIDText").GetComponent<Text>());

        menuPanels = new List<GameObject>();
        menuPanels.Add(GameObject.Find("ObjectivePanel"));
        menuPanels.Add(GameObject.Find("QBPanel"));
        menuPanels.Add(GameObject.Find("PSPanel"));
        menuPanels.Add(GameObject.Find("QAIPanel"));

        player = GameObject.Find("Sprout").GetComponent<Player>();
        hpTxt = GameObject.Find("hpText").GetComponent<Text>();
        mpTxt = GameObject.Find("mpText").GetComponent<Text>();

        hoverX = 0;
        hoverY = 1;
        inputsLocked = true;

        spells = GameObject.Find("AtkMenu5Options").GetComponent<CombatMenuController>().getSpells();
        items = GameObject.Find("Inventory").GetComponent<InventoryManager>().getItemObjects();

        //disable unwanted panels
        panelDisplays[1].SetActive(false);
        panelDisplays[2].SetActive(false);
        panelDisplays[3].SetActive(false);
    }

    void Update() {
        hpTxt.text = "HP: " + player.getCurrentHP() + "/" + player.getMaxHP();
        mpTxt.text = "MP: " + player.getCurrentMP() + "/" + player.getMaxMP();
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
                hoverY--;
            } else if(v < -stickSens) {
                hoverY++;
            }

            if(hoverY > 2) {
                hoverY = 2;
            } else if(hoverY< 0) {
                hoverY = 0;
            } else if (Math.Abs(v) > stickSens){
                pmc.playNavClip();
            }

            if(h > stickSens) {
                hoverX++;
            } else if(h < -stickSens) {
                hoverX--;
            }

            if(hoverY == 0) { //tabs
                pmc.playNavClip();
                if(hoverX < 0) {
                    hoverX = 3;
                } else if(hoverX > 3) {
                    hoverX = 0;
                }
                pmc.SetTabText(hoverX);
            } else { //not tabs
                if(hoverX > 1) {
                    hoverX = 1;
                } else if(hoverX < 0) {
                    hoverX = 0;
                } else if(Math.Abs(h) > stickSens) {
                    pmc.playNavClip();
                }
                togglePanel();
            }
            
            //set color of panels accordingly
            colorPanels();
        }
    }

    void OnUIConfirm() {
        if(!inputsLocked) {
            if(hoverY == 0) {
                pmc.playConfirmClip();
                inputsLocked = true;
                switch(hoverX) {
                    case 0: //current tab
                        inputsLocked = false;
                        hoverY = 1;
                        hoverX = 0;
                        break;
                    case 1: //spells page
                        hoverY = 1;
                        hoverX = 0;
                        pmc.enableTab("spells");
                        break;
                    case 2: //items page
                        hoverY = 1;
                        hoverX = 0;
                        pmc.enableTab("items");
                        break;
                    case 3: //options page
                        hoverY = 1;
                        hoverX = 0;
                        pmc.enableTab("options");
                        break;            
                }
            }
        }
    }

    void OnUIReturn() {
        switch(hoverY) {
            case 0:
                inputsLocked = true;
                hoverY = 1;
                hoverX = 0;
                pmc.enableTab("return");
                break;
            default: //any other spot on the menu
                pmc.playReturnClip();
                hoverY = 0;
                hoverX = 0;
                break;    
        }
    }

    private void togglePanel() {
        if(hoverY != 0) {
            //disable all panels
            panelDisplays[0].SetActive(false); //[0, 0]
            panelDisplays[1].SetActive(false); //[0, 1]
            panelDisplays[2].SetActive(false); //[1, 0]
            panelDisplays[3].SetActive(false); //[1, 1]

            switch(hoverY) {
                case 1:
                    switch(hoverX) {
                        case 0:
                            panelDisplays[0].SetActive(true);
                            //text currently static
                            break;
                        case 1: //quickbinds
                            panelDisplays[1].SetActive(true);
                            setQBText();
                            break;    
                    }
                    break;
                case 2:
                    switch(hoverX) {
                        case 0: //prepped spells
                            panelDisplays[2].SetActive(true);
                            setSpellsText();
                            break;
                        case 1: //quick access items
                            panelDisplays[3].SetActive(true);
                            setItemsText();
                            break;    
                    }
                    break;   
            }
        }
    }

    private void setQBText() {
        GameObject[] binds = GameObject.Find("Inventory").GetComponent<QuickBinds>().getQuickBinds();
        panelTexts[0].text = "";
        for(int i = 0; i < binds.Length; i++) {
            panelTexts[0].text += "Slot " + (i + 1) + " - ";
            switch(binds[i].tag) {
                case "Spell":
                    panelTexts[0].text += binds[i].GetComponent<Spell>().getName();
                    break;
                case "Item":
                    panelTexts[0].text += binds[i].GetComponent<Item>().getName();
                    break;
                default:
                    panelTexts[0].text += "Blank";
                    break;    
            }
            panelTexts[0].text += "\n\n";
        }
    }

    private void setSpellsText() {
        panelTexts[1].text = "";
        for(int i = 0; i < spells.Count; i++) {
            panelTexts[1].text += "Slot " + (i + 1) + " - " + spells[i].getName() + "\n\n";
        }
    }

    private void setItemsText() {
        panelTexts[2].text = "";
        for(int i = 0; i < items.Count; i++) {
            panelTexts[2].text += "Slot " + (i + 1) + " - " + items[i].getName() + " x " + items[i].getCount() + "\n\n";
        }
    }

    private void colorPanels() {
        for(int i = 0; i < menuPanels.Count; i++) {
            menuPanels[i].GetComponent<Image>().color = panelColors[0];
        }

        switch(hoverY) {
                case 1:
                    switch(hoverX) {
                        case 0:
                            menuPanels[0].GetComponent<Image>().color = panelColors[1];
                            break;
                        case 1: //quickbinds
                            menuPanels[1].GetComponent<Image>().color = panelColors[1];
                            break;    
                    }
                    break;
                case 2:
                    switch(hoverX) {
                        case 0: //prepped spells
                            menuPanels[2].GetComponent<Image>().color = panelColors[1];
                            break;
                        case 1: //quick access items
                            menuPanels[3].GetComponent<Image>().color = panelColors[1];
                            break;    
                    }
                    break;
                default:
                    break;       
            }
    }
}
