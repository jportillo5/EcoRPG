using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BindOption : MonoBehaviour
{
    public GameObject bind;

    private string type;
    
    // Start is called before the first frame update
    void Start()
    {
        if(bind.tag == "Spell") {
            type = "spell";
        } else if(bind.tag == "Item") {
            type = "item";
        }
    }

    public string getName() {
        switch(type) { //consider using a method returning a generic so that we can use the same named calls of methods in both scripts
            case "spell":
                return bind.GetComponent<Spell>().getName();
            case "item":
                return "Items have not been implemented yet";
            default:
                return "";     
        }
    }

    public void UseBind() {
        switch(type) {
            case "spell":
                //use the spell 
                break;
            case "item":
                //use the item
                break;   
            default:
                break;     
        }
    }
}
