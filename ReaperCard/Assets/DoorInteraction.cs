using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Door door;

    bool bHasKeyItem = true;

    void Interact(GameObject player) {
        if(bHasKeyItem)
        {
            door.OpenDoor(true);
        }
        else
        {
            // Set Dialog
        }
    }
}
