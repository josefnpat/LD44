﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this is so bad
public class InventoryLayouter : MonoBehaviour
{
    private List<GameObject> itemObjects = new List<GameObject>();
    public GameObject player;

    private Dictionary<string, InventoryItem> inventoryItems = new Dictionary<string, InventoryItem>();

    private const float ICON_SIZE_X = 0.1f;
    private const float PADDING_X = 0.02f; // expressed in terms of x but applied on both axes
    private const float SPACING = 0.02f;

    void Awake() {
        foreach(var invItem in Resources.FindObjectsOfTypeAll(typeof(InventoryItem)) as InventoryItem[]) {
            inventoryItems[invItem.name] = invItem;
        }
    }

    void layout() {
        var totalSize = GetComponent<RectTransform>().sizeDelta;
        var xyFactor = totalSize.x / totalSize.y;
        var totalWidthPct = itemObjects.Count * ICON_SIZE_X
            + (itemObjects.Count - 1) * SPACING + PADDING_X;
        var totalWidth = totalWidthPct * totalSize.x;
        var y = (-0.5f + PADDING_X * xyFactor) * totalSize.y;
        var x = 0.5f * totalSize.x - totalWidth;
        foreach(var item in itemObjects) {
            var trafo = item.GetComponent<RectTransform>();
            trafo.pivot = new Vector2(0f, 0f);
            trafo.localPosition = new Vector3(x, y, 0f);
            x += totalSize.x * (ICON_SIZE_X + SPACING);
            var imgSize = trafo.sizeDelta;
            trafo.localScale = new Vector3(
                ICON_SIZE_X * totalSize.x / imgSize.x,
                ICON_SIZE_X * xyFactor * totalSize.y / imgSize.y,
                1f);
        }
    }

    void Update() {
        var items = player.GetComponent<Inventory>().getItems();

        for(var i = 0; i < items.Count; ++i) {
            // there is an item, but it's the wrong one
            if(itemObjects.Count > i &&
                    itemObjects[i].GetComponent<UiInventoryItem>().item.name != items[i]) {
                Destroy(itemObjects[i]);
                itemObjects[i] = null;
            }

            // there is no item yet
            if(i >= itemObjects.Count) {
                itemObjects.Add(null);
                Debug.Assert(itemObjects.Count > i);
            }

            if(itemObjects[i] == null) {
                Debug.Assert(inventoryItems.ContainsKey(items[i]));
                var invItem = inventoryItems[items[i]];
                var obj = new GameObject();
                obj.transform.parent = transform;
                obj.AddComponent<UiInventoryItem>().item = invItem;
                obj.AddComponent<Image>().sprite = invItem.uiSprite;
                itemObjects[i] = obj;
            }
        }

        layout();
    }
}