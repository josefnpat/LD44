using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFacingDirection
{
    Left,
    Right
}

/*
 *  Actor
 *  Represents a character in the world.
 *  Serves as the basis for the player and NPC classes.
 *
 * 
 *  Relevant functionality:
 *        AddMovementInput(Vector3 Direction, float Scale)
 * 
 */ 

    
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Actor : MonoBehaviour
{
    CapsuleCollider Capsule;
    Rigidbody Body;

    Vector3 PendingVelocity;

    [Header("Movement Settings")]
    float WalkSpeed = 100;

    // Start is called before the first frame update
    void Start()
    {
        Capsule = GetComponent<CapsuleCollider>();
        Body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyVelocity();
    }

    // Queues up movement for the next movement update, preferably a unit vector
    public void AddMovementInput(Vector3 Direction, float Scale)
    {
        PendingVelocity += Direction * Scale;
    }

    public void ApplyVelocity()
    {
        Body.MovePosition(transform.position + PendingVelocity * WalkSpeed * Time.deltaTime);
    }
}
