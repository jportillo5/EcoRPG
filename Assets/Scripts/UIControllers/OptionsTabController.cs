using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsTabController : MonoBehaviour
{
    public float stickSens; //determines the threshold for navigating menus with the stick
    public List<Color> panelColors;
    /*
        0- volume panel default
        1- volume panel hover
        2- quit panel default
        3- quit panel hover
        4- Submenu default
        5- Submenu hover
    */

    List<Image> panelImages; //images we need to set the color of
    /*
        0- music volume panel
        1- sound effects volume panel
        2- quit game panel
        3- submenu continue panel
        4- submenu quit panel
    */

    GameObject subMenuPanel;
    private Slider musicVolume;
    private Slider sfxVolume;
    public float musicScale; //getter called by other scripts to calculate output volume of music
    public float sfxScale; //getter called by other scripts to calculate output volume of sound effects

    private int hoverX;
    private int hoverY;
    private int subHover;
    private string submenu;

    private bool inputsLocked;
    private bool navLocked; //prevents certain menu interactions from being made too fast

    PauseMenuController pmc;
    
    // Start is called before the first frame update
    void Start()
    {
        pmc = GameObject.Find("PauseMenu").GetComponent<PauseMenuController>();
        
        panelImages = new List<Image>();
        panelImages.Add(GameObject.Find("MVPanel").GetComponent<Image>());
        panelImages.Add(GameObject.Find("SVPanel").GetComponent<Image>());
        panelImages.Add(GameObject.Find("QuitPanel").GetComponent<Image>());
        panelImages.Add(GameObject.Find("ContinuePanel").GetComponent<Image>());
        panelImages.Add(GameObject.Find("QuitGamePanel").GetComponent<Image>());

        subMenuPanel = GameObject.Find("QuitSubMenu");

        musicVolume = GameObject.Find("MusicVolume").GetComponent<Slider>();
        sfxVolume = GameObject.Find("SfxVolume").GetComponent<Slider>();

        hoverX = 3;
        hoverY = 1;
        subHover = 0;
        submenu = "none";
        inputsLocked = true;

        subMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(hoverY != 0) {
            hoverX = 3;
        }
    }

    public void lockInputs() {
        inputsLocked = true;
    }

    public void unlockInputs() {
        inputsLocked = false;
    }

    public void unlockNavChange() {
        navLocked = false;
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

            if(hoverY > 3) {
                hoverY = 3;
            } else if(hoverY < 0) {
                hoverY = 0;
            }

            if(hoverY == 0) {
                if(h < -stickSens) {
                    hoverX--;
                } else if(h > stickSens) {
                    hoverX++;
                }

                if(hoverX < 0) {
                    hoverX = 3;
                } else if(hoverX > 3) {
                    hoverX = 0;
                } //loop around

                pmc.SetTabText(hoverX);
            } else if(hoverY == 1 && !navLocked) {//music volume
                if(h != 0 && Math.Abs(h) > stickSens) {
                    navLocked = true;
                    Invoke("unlockNavChange", 0.5f);

                    if(h < 0) { //check for stick sensitivity threshold has already been passed
                        musicScale -= .1f;
                    } else if(h > 0) {
                        musicScale += .1f;
                    }

                    if(musicScale < 0f) {
                        musicScale = 0f;
                    } else if(musicScale > 1f) {
                        musicScale = 1f;
                    }

                    updateMusicSlider(); 
                }
            } else if(hoverY == 2 && !navLocked) {//sound effects 
                if(h != 0 && Math.Abs(h) > stickSens) {
                    navLocked = true;
                    Invoke("unlockNavChange", 0.5f);

                    if(h < 0) {
                        sfxScale -= .1f;
                    } else if(h > 0) {
                        sfxScale += .1f;
                    }

                    if(sfxScale < 0f) {
                        sfxScale = 0f;
                    } else if(sfxScale > 1f) {
                        sfxScale = 1f;
                    } 

                    updateSfxSlider();
                }
            }
        } else if(submenu == "quit") {
            if(h != 0 && Math.Abs(h) > stickSens && !navLocked) {
                if(subHover == 0) {
                    subHover = 1;
                } else {
                    subHover = 0;
                }
            }
        }
        setPanelColors();
    }

    void OnUIConfirm() {
        if(!inputsLocked) {
            switch (hoverY) {
                case 0: //tabs
                    inputsLocked = true;
                    switch(hoverX) { //figure out which menu to go back to
                        case 0: //status page
                            hoverY = 1;
                            hoverX = 3;
                            pmc.enableTab("status");
                            break;
                        case 1: //spells page
                            hoverY = 1;
                            hoverX = 3;
                            pmc.enableTab("spells");
                            break;
                        case 2: //items tab
                            hoverY = 1;
                            hoverX = 3;
                            pmc.enableTab("items");
                            break;
                        case 3: //current tab
                            inputsLocked = false;
                            hoverY = 1;
                            break;          
                    }
                    break;
                case 3: //submenu
                    inputsLocked = true;
                    submenu = "quit";
                    subHover = 0;
                    subMenuPanel.SetActive(true);
                    break;
                default:
                    break;        
            }
        } else if(submenu == "quit") {
            switch(subHover) {
                case 0: //return to previous menu
                    subMenuPanel.SetActive(false);
                    subHover = 0;
                    submenu = "none";
                    inputsLocked = false;
                    break;
                case 1:
                    //disable everything and transition to main menu scene
                    Debug.Log("This would hypothetically quit the game");
                    break;    
            }
        }
    }

    void OnUIReturn() {
        switch(submenu) {
            case "none":
                if(hoverY == 0) {
                    hoverY = 1;
                    hoverX = 3;
                    inputsLocked = true;
                    pmc.enableTab("return");
                }
                break;
            case "quit":
                subMenuPanel.SetActive(false);
                subHover = 0;
                submenu = "none";
                inputsLocked = false;
                break;
        }
    }

    private void updateMusicSlider() {
        musicVolume.value = musicScale;
    }

    private void updateSfxSlider() {
        sfxVolume.value = sfxScale;

        //call setVolume methods in all objects with an audio source
        pmc.setVolume(sfxScale);
        GameObject.Find("Weapon").GetComponent<WeaponController>().setVolume(sfxScale);
        GameObject.Find("Sprout").GetComponent<Player>().setVolume(sfxScale);
    }

    public float getMusicScale() {
        return musicScale;
    }

    public float getSfxScale() {
        return sfxScale;
    }

    public void setPanelColors() {
        switch(submenu) {
            case "none"://only dealing with panels 0-2
                //reset all panels to normal
                panelImages[0].color = panelColors[0];
                panelImages[1].color = panelColors[0];
                panelImages[2].color = panelColors[2];
                switch(hoverY) {
                    case 1: //music volume
                        panelImages[0].color = panelColors[1];
                        break;
                    case 2: //sfx volume
                        panelImages[1].color = panelColors[1];
                        break;
                    case 3: //quit panel
                        panelImages[2].color = panelColors[3];
                        break;
                    default:
                        break;            
                }
                break;
            case "quit"://only dealing with panels 3 and 4
                //reset panels to normal
                panelImages[3].color = panelColors[4];
                panelImages[4].color = panelColors[4];
                switch(subHover) {
                    case 0:
                        panelImages[3].color = panelColors[5];
                        break;
                    case 1:
                        panelImages[4].color = panelColors[5];
                        break;    
                }
                break;    
        }
    }
}
