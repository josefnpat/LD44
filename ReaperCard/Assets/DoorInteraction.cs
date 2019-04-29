using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject dialogManager;
    public Door door;
    public string closedMessage;
    public List<InventoryItem> keys; // you need all of them

    private GameObject player;
    private bool waitingForDialogManager;

    void Interact(GameObject player) {
        var inventory = player.GetComponent<Inventory>();
        bool hasAllKeys = true;
        foreach(var key in keys) {
            if(!inventory.hasItem(key.itemName)) {
                hasAllKeys = false;
                break;
            }
        }

        if(hasAllKeys) {
            door.OpenDoor(true);
            SoundManager.instance.Play("dooropen");
        } else {
            dialogManager.GetComponent<DialogManagerUI>().SetEvent(closedMessage);
            player.GetComponent<Actor>().SetState(EActorState.InConversation);
            this.player = player;
            waitingForDialogManager = true;
            SoundManager.instance.Play("doorclosed");
        }
    }

    void Update() {
        if(waitingForDialogManager && dialogManager.GetComponent<DialogManagerUI>().ReadyForNext()) {
            player.GetComponent<Actor>().SetState(EActorState.Walking);
            waitingForDialogManager = false;
        }
    }
}
