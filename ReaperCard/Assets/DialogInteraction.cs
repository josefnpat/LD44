using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteraction : MonoBehaviour
{
    public GameObject dialogManager;
    public Dialog dialog;

    void Interact(GameObject player) {
        dialogManager.GetComponent<DialogManager>().setDialog(dialog.getRoot(), player, gameObject);
    }
}
