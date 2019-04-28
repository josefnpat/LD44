using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactible : MonoBehaviour
{
    public GameObject DialogReadyIcon;
    SphereCollider interactionArea;

    public UnityEvent Interact;

    // Start is called before the first frame update
    void Start()
    {
        interactionArea = GetComponent<SphereCollider>();

    }

    private void OnTriggerEnter(Collider Other)
    {
        DialogReadyIcon.SetActive(true);

        PlayerComponent player = Other.GetComponent<PlayerComponent>();
        if(player)
        {
            player.TrySetInteractObj(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider Other)
    {
        DialogReadyIcon.SetActive(false);

        PlayerComponent player = Other.GetComponent<PlayerComponent>();
        if (player)
        {
            player.TryClearInteractObj(this.gameObject);
        }
    }
}
