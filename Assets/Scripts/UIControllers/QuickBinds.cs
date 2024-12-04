using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickBinds : MonoBehaviour
{
    public GameObject[] binds;
    private PlayerController player;

    void Start() {
        player = GetComponentInParent<PlayerController>();
    }

    public GameObject[] getQuickBinds() {
        return binds;
    }

    public void removeBind(int index) {
        binds[index] = null;
    }

    public void setBind(int index, GameObject bind) {
        binds[index] = bind;
    }

    public GameObject getBind(int index) {
        return binds[index];
    }

    public void Use(int index) {
        switch(binds[index].tag) {
            case "Spell":
                binds[index].GetComponent<Spell>().instantiateAttack(player.getDirection(), player.GetComponentInParent<Transform>());
                break;
            case "Item":
                binds[index].GetComponent<Item>().useItem();
                break;
            default:
                break;    
        }
    }
}
