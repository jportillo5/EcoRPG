using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BindOption : MonoBehaviour //DO NOT USE THIS SCRIPT, IT DOES NOT WORK AS INTENDED AND WAS COMPLETELY UNNECCESSARY
{
    public GameObject bind;

    private string type;

    private PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        if(bind.tag == "Spell") {
            type = "spell";
        } else if(bind.tag == "Item") {
            type = "item";
        }

        player = GetComponentInParent<PlayerController>();
    }

    public string getName() {
        switch(type) { //consider using a method returning a generic so that we can use the same named calls of methods in both scripts
            case "spell":
                return bind.GetComponent<Spell>().getName();
            case "item":
                return bind.GetComponent<Item>().getName();
            default:
                return "Blank";     
        }
    }

    public void UseBind() {
        switch(type) {
            case "spell":
                bind.GetComponent<Spell>().instantiateAttack(player.getDirection(), player.GetComponentInParent<Transform>());
                break;
            case "item":
                bind.GetComponent<Item>().useItem();
                break;   
            default:
                break;     
        }
    }
}
