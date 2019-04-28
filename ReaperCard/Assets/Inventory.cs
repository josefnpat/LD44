using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class InventoryEntry
    {
        public InventoryItem item;
        public bool has;
    }
    public List<InventoryEntry> items = new List<InventoryEntry>();
    public GameObject itemAnimPrefab;

    void Start()
    {
    }

    void Update()
    {
    }

    // where to put this?
    void spawnAnimItem(string itemName)
    {
        var item = InventoryItem.getByName(itemName);
        Debug.Assert(item, "Unknown item of type " + itemName);
        var obj = Instantiate(itemAnimPrefab, transform);
        obj.SendMessage("Init", item);
    }

    public void addItem(string itemName, bool forceAnim = false)
    {
        bool hasAlready = hasItem(itemName);
        if (!hasAlready) items[getItemIndex(itemName)].has = true;
        if (!hasAlready || forceAnim) spawnAnimItem(itemName);
        // TODO: play sound
        // player changes sprite?
        // TODO: GetComponent<Actor>().SetState(EActorState.ReceivingItem);
    }

    public void removeItem(string itemName)
    {
        items[getItemIndex(itemName)].has = false;
    }

    public bool hasItem(string itemName)
    {
        return items[getItemIndex(itemName)].has;
    }

    public List<InventoryItem> getItems()
    {
        return items.Where(entry => entry.has).Select(entry => entry.item).ToList();
    }

    private int getItemIndex(string name)
    {
        var idx = items.FindIndex(entry => entry.item.itemName == name);
        Debug.Assert(idx >= 0, "Could not find item with name " + name);
        return idx;
    }
}
