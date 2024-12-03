using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour //This script handles change from core menu tabs as well as pausing and unpausing the game
{
    //Tabs
    List<GameObject> tabs;
    List<Text> tabTexts;
    //flags
    /*
    Menu flags:
    status - main status page, uses StatusTabController.
    spells - spells page, uses SpellTabController
    items - items page, uses ItemTabController
    options - options page, uses OptionsTabController
    */

    // Start is called before the first frame update
    void Start()
    {
        tabs = new List<GameObject>();
        tabs.Add(GameObject.Find("StatusTab"));
        tabs.Add(GameObject.Find("SpellsTab"));
        tabs.Add(GameObject.Find("ItemsTab"));
        tabs.Add(GameObject.Find("OptionsTab"));

        tabTexts = new List<Text>();
        tabTexts.Add(GameObject.Find("StatusTabText").GetComponent<Text>());
        tabTexts.Add(GameObject.Find("SpellsTabText").GetComponent<Text>());
        tabTexts.Add(GameObject.Find("ItemsTabText").GetComponent<Text>());
        tabTexts.Add(GameObject.Find("OptionsTabText").GetComponent<Text>());
        enableTab("return"); //on wakeup, ensures the menu is closed
    }

    public void openMenu() {
        //for now, I'm not gonna have enemies pause when opening up the menu,
        //since it invalidates having items be accessible from the combat menu
        //This may be added into a later version as a sort of difficulty setting
        enableTab("status");
    }

    public void enableTab(string tabName) {
        for(int i = 0; i < tabs.Count; i++) {
            tabs[i].SetActive(false);
        }
        
        switch(tabName) { //all tabs should be in default state after exiting them
            case "status":
                tabs[0].SetActive(true);
                break;
            case "spells":
                tabs[1].SetActive(true);
                break;
            case "items": 
                tabs[2].SetActive(true);
                break;  
            case "return": //return to the game
                GameObject.Find("Sprout").GetComponent<PlayerController>().UnlockMovement();
                break;      
        }
    }

    public void SetTabText(int hover) {
        for(int i = 0; i < tabTexts.Count; i++) {
            tabTexts[i].text = "";
            if(hover == i) {
                tabTexts[i].text = ">";
            }
        }
        tabTexts[0].text += "Status";
        tabTexts[1].text += "Spells";
        tabTexts[2].text += "Items";
        tabTexts[3].text += "Options";
    }
    
}
