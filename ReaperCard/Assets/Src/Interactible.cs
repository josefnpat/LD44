using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactible : MonoBehaviour
{
    SphereCollider interactionArea;

    public UnityEvent Interact;

    // Start is called before the first frame update
    void Start()
    {
        interactionArea = GetComponent<SphereCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Interact == null)
        {
            Interact = new UnityEvent();
        }
        
    }

    private void OnTriggerEnter(Collider Other)
    {
        PlayerComponent player = Other.GetComponent<PlayerComponent>();
        if(player)
        {
            player.TrySetInteractObj(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider Other)
    {
        PlayerComponent player = Other.GetComponent<PlayerComponent>();
        if (player)
        {
            player.TryClearInteractObj(this.gameObject);
        }
    }
}
