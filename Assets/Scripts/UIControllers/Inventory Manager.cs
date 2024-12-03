using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    QuickBinds quickBinds;
    public List<GameObject> items; //ItemSlot objects
    
    // Start is called before the first frame update
    void Start()
    {
        quickBinds = gameObject.GetComponent<QuickBinds>(); 
    }

    //Methods that interact with the quickbinds




    //Methods that interact with the items
    public List<string> getItemNames() {
        List<string> names = new List<string>();
        foreach (GameObject slot in items) {
            names.Add(slot.GetComponent<ItemSlot>().getItemName());
        }
        return names;
    }

    public List<Item> getItemObjects() {
        List<Item> objects = new List<Item>();
        foreach (GameObject slot in items) {
            objects.Add(slot.GetComponent<ItemSlot>().item);
        }
        return objects;
    }
    
    public int FindItemCount(string itemName) {
        Debug.Log("Searching for item with name " + itemName + " amongst item slot objects in inventory");
        for(int i = 0; i < items.Count; i++) {
            if(items[i].GetComponent<ItemSlot>().getItemName() == itemName) {
                return items[i].GetComponent<ItemSlot>().getItemCount();
            }
        }
        return -1;
    }


    //Methods interacting with a singularitem. In the future, the system will need to be
    //made much more robust than this, but for now with only two items this is fine.
    public Sprite getItemSprite(int index) {
        return items[index].GetComponent<ItemSlot>().getItemSprite();
    }

    public void useItem(int index) { //system will need to be made a little more robust than this
    //but for now, with only two items in the game, this is perfectly reasonable
        items[index].GetComponent<ItemSlot>().getItem().useItem();
    }

    public void reduceItemCount(string itemName, int rem) {
        for(int i = 0; i < items.Count; i++) {
            if(items[i].GetComponent<ItemSlot>().getItemName() == itemName) {
                items[i].GetComponent<ItemSlot>().removeCount(rem);
            }
        }
    }
}
