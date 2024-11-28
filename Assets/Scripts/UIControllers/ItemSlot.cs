using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public Item item;
    public int count; //number of this item currently available in
    
    public string getItemName() {
        return item.name;
    }

    public string getItemDescription() {
        return item.description;
    }

    public Sprite getItemSprite() {
        return item.getIcon();
    }

    public int getItemCount() {
        return count;
    }

    public void addCount(int add) {
        count += add;
    }

    public void removeCount(int rem) {
        count -= rem;
        if (count < 0) { //additional systems should automatically make it so that getting a count lower than 0 is impossible, but
        //this check is here just in case
            count = 0;
        }
    }
}
