using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
public class Interactible : MonoBehaviour
{
    CapsuleCollider capsule;

    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        
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
