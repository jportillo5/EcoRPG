using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class MPBarController : MonoBehaviour
{
    
    private float maxMP;
    private float currentMP;

    private Slider slider;
    private string state;
    private Player player;
    public Color normalColor;
    public Color recoveringColor;

    void Start() {
        player = GameObject.Find("Sprout").GetComponent<Player>();
        maxMP = player.getMaxMP();
        currentMP = maxMP;
        state = "normal";
        slider = gameObject.GetComponent<Slider>();
    }

    public void UpdateMPBar(float mp) {
        Debug.Log("Updating MP Bar");
        currentMP = mp;
        if(currentMP >= maxMP) {
            currentMP = maxMP;
            state = "normal";
            setColor();
        } else if(currentMP <= 0f) {
            currentMP = 0;
            state = "recovering";
            setColor();
        }
        slider.value = currentMP/maxMP;

        if(state == "recovering") {
            player.BeginMPRecovery();
        }
    }

    void setColor() { //changes the color of the healthbar based on the bar's state
        switch(state) {
            case "normal":
                GameObject.Find("Fill").GetComponent<Image>().color = normalColor;
                break;
            case "recovering":
                GameObject.Find("Fill").GetComponent<Image>().color = recoveringColor;
                break;    
        }
    }

    public string getState() {
        return state;
    }
}
