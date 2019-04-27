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
 *	
 * 
 */ 

	
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Actor : MonoBehaviour
{
	CapsuleCollider Capsule;
	Rigidbody Body;

	Vector3 PendingVelocity;

	// Start is called before the first frame update
	void Start()
	{
		Capsule = GetComponent<CapsuleCollider>();
		Body = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	// Queues up movement for the next movement update, preferably a unit vector
	void AddMovementInput(Vector3 Direction, float Scale)
	{
		PendingVelocity += Direction * Scale;
	}

	void ApplyVelocity()
	{

	}
}
