using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    public InventoryItem item;

    void Interact(GameObject player) {
        player.GetComponent<Inventory>().addItem(item.itemName);
        var itemObject = gameObject.transform.parent.gameObject;
        Destroy(itemObject);
        SoundManager.instance.Play("itempickup");
    }
}
