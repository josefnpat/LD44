using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteraction : MonoBehaviour
{
    public GameObject dialogManager;
    public Dialog dialog;

    void Start() {
        Interact(); // TESTING
    }

    void Interact() {
        dialogManager.GetComponent<DialogManager>().setDialog(dialog.getRoot());
    }
}
