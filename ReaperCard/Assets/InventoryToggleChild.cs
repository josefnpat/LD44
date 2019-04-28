using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggleChild : MonoBehaviour
{
    [System.Serializable]
    public struct ChildToggle {
        public GameObject child;
        public string item;
    };

    public List<ChildToggle> toggles;

    void Update() {
        foreach(var toggle in toggles) {
            toggle.child.SetActive(GetComponent<Inventory>().hasItem(toggle.item));
        }
    }
}
