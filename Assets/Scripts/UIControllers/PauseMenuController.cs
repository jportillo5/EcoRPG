using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour //This script handles change from core menu tabs as well as pausing and unpausing the game
{
    //Tabs
    List<GameObject> tabs;
    List<GameObject> tabTexts;
    
    //Sound Effects
    public AudioClip navClip;
    public AudioClip confirmClip;
    public AudioClip cancelClip;
    public AudioClip openMenuClip;
    public AudioClip closeMenuClip;

    AudioSource myAudio; //should be working, for some reason doesn't
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Player>();
        tabs = new List<GameObject>();
        tabs.Add(GameObject.Find("StatusTab"));
        tabs.Add(GameObject.Find("SpellsTab"));
        tabs.Add(GameObject.Find("ItemsTab"));
        tabs.Add(GameObject.Find("OptionsTab"));

        tabTexts = new List<GameObject>();
        tabTexts.Add(GameObject.Find("StatusTabText"));
        tabTexts.Add(GameObject.Find("SpellsTabText"));
        tabTexts.Add(GameObject.Find("ItemsTabText"));
        tabTexts.Add(GameObject.Find("OptionsTabText"));
        enableTab("return"); //on wakeup, ensures the menu is closed
    }

    void OnTestSound() {
        Debug.Log("Currently on UI input map");
    }

    public void openMenu() {
        //for now, I'm not gonna have enemies pause when opening up the menu,
        //since it invalidates having items be accessible from the combat menu
        //This may be added into a later version as a sort of difficulty setting
        //myAudio.PlayOneShot(openMenuClip);
        player.playClip(openMenuClip);
        enableTab("status");
    }

    public void enableTab(string tabName) {
        for(int i = 0; i < tabs.Count; i++) {
            tabs[i].SetActive(false);
            tabTexts[i].SetActive(true);
        }
        
        switch(tabName) { //all tabs should be in default state after exiting them
            case "status":
                tabs[0].SetActive(true);
                tabs[0].GetComponent<StatusTabController>().unlockInputs();
                break;
            case "spells":
                tabs[1].SetActive(true);
                tabs[1].GetComponent<SpellTabController>().unlockInputs();
                break;
            case "items": 
                tabs[2].SetActive(true);
                tabs[2].GetComponent<ItemTabController>().unlockInputs();
                break;
            case "options":
                tabs[3].SetActive(true);
                tabs[3].GetComponent<OptionsTabController>().unlockInputs();
                break;      
            case "return": //return to the game
                for(int i = 0; i < tabs.Count; i++) {
                    tabs[i].SetActive(false);
                    tabTexts[i].SetActive(false);
                }
                //myAudio.PlayOneShot(closeMenuClip);
                player.playClip(closeMenuClip);
                GameObject.Find("Sprout").GetComponent<PlayerController>().regainPlayerControl();
                break;      
        }
    }

    public void SetTabText(int hover) {
        for(int i = 0; i < tabTexts.Count; i++) {
            tabTexts[i].GetComponent<Text>().text = "";
            if(hover == i) {
                tabTexts[i].GetComponent<Text>().text = ">";
            }
        }
        tabTexts[0].GetComponent<Text>().text += "Status";
        tabTexts[1].GetComponent<Text>().text += "Spells";
        tabTexts[2].GetComponent<Text>().text += "Items";
        tabTexts[3].GetComponent<Text>().text += "Options";
    }

    public void playConfirmClip() {
        //myAudio.PlayOneShot(confirmClip);
        player.playClip(confirmClip);
    }

    public void playReturnClip() {
        //myAudio.PlayOneShot(cancelClip);
        player.playClip(cancelClip);
    }

    public void playNavClip() {
        //myAudio.PlayOneShot(navClip);
        player.playClip(navClip);
    }

    public void setVolume(float volume) {
        //myAudio.volume = volume;
    }
    
}
