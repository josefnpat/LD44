using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this is so bad
public class InventoryLayouter : MonoBehaviour
{
    private class ItemData
    {
        public GameObject itemObject = null;
        public float scale = 1.0f;
    }
    private List<ItemData> items = new List<ItemData>();
    public GameObject player;

    private const float ICON_SIZE_X = 0.1f;
    private const float PADDING_X = 0.02f; // expressed in terms of x but applied on both axes
    private const float SPACING = 0.02f;
    private const float SCALE_UP_SPEED = 4.0f;
    private const float SCALE_DOWN_SPEED = 4.0f;
    private const float MAX_SCALE = 2.0f;

    void resetLayout()
    {
        foreach (var item in items) item.scale = 1.0f;
    }

    void updateLayout()
    {
        var totalSize = GetComponent<RectTransform>().sizeDelta;
        var mousePos = Input.mousePosition - new Vector3(totalSize.x, totalSize.y, 0f) * 0.5f;
        var xyFactor = totalSize.x / totalSize.y;

        var totalWidthPct = 0f;
        foreach (var item in items) totalWidthPct += ICON_SIZE_X * item.scale + SPACING;
        totalWidthPct -= SPACING;
        totalWidthPct += PADDING_X;
        var totalWidth = totalWidthPct * totalSize.x;

        var y = (-0.5f + PADDING_X * xyFactor) * totalSize.y;
        var x = 0.5f * totalSize.x - totalWidth;
        foreach (var item in items)
        {
            var trafo = item.itemObject.GetComponent<RectTransform>();
            trafo.pivot = new Vector2(0f, 0f);
            trafo.localPosition = new Vector3(x, y, 0f);
            var imgSize = trafo.sizeDelta;
            trafo.localScale = new Vector3(
                ICON_SIZE_X * totalSize.x / imgSize.x * item.scale,
                ICON_SIZE_X * xyFactor * totalSize.y / imgSize.y * item.scale,
                1f);
            var screenImgSize = new Vector2(imgSize.x * trafo.localScale.x,
                imgSize.y * trafo.localScale.y);

            var inRect = mousePos.x > x && mousePos.x < x + screenImgSize.x
                && mousePos.y > y && mousePos.y < y + screenImgSize.y;

            x += totalSize.x * (ICON_SIZE_X * item.scale + SPACING);
            if (inRect)
                item.scale += SCALE_UP_SPEED * Time.deltaTime;
            else
                item.scale -= SCALE_DOWN_SPEED * Time.deltaTime;
            item.scale = Mathf.Clamp(item.scale, 1f, MAX_SCALE);

        }
    }

    void Update()
    {
        var inventoryItems = player.GetComponent<Inventory>().getItems();

        for (var i = 0; i < inventoryItems.Count; ++i)
        {
            // there is an item, but it's the wrong one
            if (items.Count > i &&
                items[i].itemObject.GetComponent<UiInventoryItem>().item != inventoryItems[i])
            {
                Destroy(items[i].itemObject);
                items[i].itemObject = null;
            }

            // there is no item yet
            if (i >= items.Count)
            {
                items.Add(new ItemData());
                Debug.Assert(items.Count > i);
            }

            if (items[i].itemObject == null)
            {
                var obj = new GameObject();
                obj.transform.parent = transform;
                obj.AddComponent<UiInventoryItem>().item = inventoryItems[i];
                obj.AddComponent<Image>().sprite = inventoryItems[i].uiSprite;
                items[i].itemObject = obj;
                resetLayout();
            }
        }

        // delete extra items
        var numExtra = items.Count - inventoryItems.Count;
        if (numExtra > 0)
        {
            for (var i = inventoryItems.Count; i < items.Count; ++i)
                Destroy(items[i].itemObject);
            items.RemoveRange(inventoryItems.Count, numExtra);
            resetLayout();
        }

        updateLayout();
    }
}
