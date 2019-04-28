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

    public void Interact() {
        dialogManager.GetComponent<DialogManager>().setDialog(dialog.getRoot());
    }
}
