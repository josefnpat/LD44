using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<string> items = new List<string>();

    public GameObject itemAnimPrefab;

    void Start() {
        //items = Game.GInstance.gameState.inventory;
        addItem("fart");
        addItem("bird");
    }

    void Update() {
    }

    // where to put this?
    void spawnAnimItem(string itemName) {
        var item = InventoryItem.getByName(itemName);
        Debug.Assert(item);
        var obj = Instantiate(itemAnimPrefab, transform);
        obj.SendMessage("Init", item);
    }

    public void addItem(string itemName, bool forceAnim = false) {
        bool hasAlready = hasItem(itemName);
        if(!hasAlready) items.Add(itemName);
        if(!hasAlready || forceAnim) spawnAnimItem(itemName);
        // TODO: play sound
        // TODO: GetComponent<Controller>().setState(ACQUIRE_ITEM); // player changes sprite, turns around
        //Game.GInstance.gameState.inventory = items;
    }

    public void removeItem(string item, bool all = false) {
        if(all) {
            items.RemoveAll(elem => elem == item);
        } else {
            items.Remove(item);
        }
        //Game.GInstance.gameState.inventory = items;
    }

    public bool hasItem(string item) {
        return items.Contains(item);
    }

    public List<string> getItems() {
        return items;
    }
}
