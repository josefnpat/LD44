﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalExitInteraction : MonoBehaviour
{
    public GameObject dialogManager;

    void Interact(GameObject player) {
        var inventory = player.GetComponent<Inventory>();
        var allCards = inventory.hasItem("msfox") && inventory.hasItem("oldfrog")
            && inventory.hasItem("mrbird") && inventory.hasItem("youngrabbit");
        var door = gameObject.transform.parent.gameObject;
        if(allCards) {
            Destroy(door);
        } else {
            dialogManager.GetComponent<DialogManagerUI>().SetEvent("Your work is not done yet!");
        }
    }
}
