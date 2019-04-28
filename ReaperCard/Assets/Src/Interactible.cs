using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactible : MonoBehaviour
{
    public GameObject readyIcon;
    SphereCollider interactionArea;

    // Start is called before the first frame update
    void Start()
    {
        interactionArea = GetComponent<SphereCollider>();
    }

    public void doInteraction(GameObject player) {
        SendMessage("Interact", player);
    }

    private void OnTriggerEnter(Collider other) {
        readyIcon.SetActive(true);

        Interactor interactor = other.GetComponent<Interactor>();
        if(interactor) interactor.setInteractable(this.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        readyIcon.SetActive(false);

        Interactor interactor = other.GetComponent<Interactor>();
        if(interactor) interactor.clearInteractable(this.gameObject);
    }
}
