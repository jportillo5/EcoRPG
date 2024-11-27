using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    public string itemName;
    public string type;
    public float power;
    public string description;
    public AudioClip audioClip;
    // Start is called before the first frame update
    
    public string getName() {
        return itemName;
    }

    public string getType() {
        return type;
    }

    public float getPower() {
        return power;
    }

    public string getDescription() {
        return description;
    }

    public Sprite getIcon() {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public AudioClip getAudio() {
        return audioClip;
    }

    public int getCount() {
        InventoryManager invManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        return invManager.FindItemCount(itemName);
    }

    public void useItem() {
        //make sure item is in inventory
        InventoryManager invManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        if(invManager.FindItemCount(itemName) != 0) {
            //reduce item count by 1 in inventory if available
            invManager.reduceItemCount(itemName, 1);
            //then use item
            Player player = GameObject.Find("Sprout").GetComponent<Player>();
            PlayerController pc = GameObject.Find("Sprout").GetComponent<PlayerController>();
            //pc.lockMovementNoUnlock();
            pc.useItemAnimation();
            switch(type) {
                case "hp":
                    player.Heal(power);
                    break;
                case "mp":
                    player.recoverMP(power);
                    break;
                default:
                    Debug.Log("Type not recognized");
                    break;
            }
        }
    }
}
