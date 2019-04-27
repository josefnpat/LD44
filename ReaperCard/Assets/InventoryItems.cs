using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory Item")]
public class InventoryItem : ScriptableObject {
    public string name;
    public Sprite uiSprite;
    public Texture2D texture;

    public static InventoryItem getByName(string name) {
        foreach(var invItem in Resources.FindObjectsOfTypeAll(typeof(InventoryItem)) as InventoryItem[]) {
            if(invItem.name == name) return invItem;
        }
        return null;
    }
}

