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

    public float iconSizeX = 0.05f;
    public float paddingX = 0.02f; // expressed in terms of x but applied on both axes
    public float spacing = 0.02f;
    public float scaleUpSpeed = 4.0f;
    public float scaleDownSpeed = 4.0f;
    public float maxScale = 2.0f;

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
        foreach (var item in items) totalWidthPct += iconSizeX * item.scale + spacing;
        totalWidthPct -= spacing;
        totalWidthPct += paddingX;
        var totalWidth = totalWidthPct * totalSize.x;

        var y = (-0.5f + paddingX * xyFactor) * totalSize.y;
        var x = 0.5f * totalSize.x - totalWidth;
        foreach (var item in items)
        {
            var trafo = item.itemObject.GetComponent<RectTransform>();
            trafo.pivot = new Vector2(0f, 0f);
            trafo.localPosition = new Vector3(x, y, 0f);
            var spriteRect = item.itemObject.GetComponent<Image>().sprite.rect;
            trafo.sizeDelta = new Vector2(spriteRect.width, spriteRect.height);
            var imgSize = trafo.sizeDelta;
            var scale = iconSizeX * item.scale * totalSize.x / imgSize.x;
            trafo.localScale = new Vector3(scale, scale, 1f);
            var screenImgSize = new Vector2(imgSize.x * trafo.localScale.x,
                imgSize.y * trafo.localScale.y);

            var inRect = mousePos.x > x && mousePos.x < x + screenImgSize.x
                && mousePos.y > y && mousePos.y < y + screenImgSize.y;

            x += totalSize.x * (iconSizeX * item.scale + spacing);
            if (inRect)
                item.scale += scaleUpSpeed * Time.deltaTime;
            else
                item.scale -= scaleDownSpeed * Time.deltaTime;
            item.scale = Mathf.Clamp(item.scale, 1f, maxScale);
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
