using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFacingDirection
{
    Left,
    Right
}

public enum EDoesFloat
{
    DoesNotFloat,
    Floats
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

public enum EActorState
{
    Walking,
    ReceivingItem,
    InConversation
}

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Actor : MonoBehaviour
{
    CapsuleCollider capsule;
    Rigidbody body;
    public GameObject LeftPlane;
    public GameObject RightPlane;

    private EActorState CurrentState = EActorState.Walking;
    private Vector3 pendingMovement = new Vector3();
    public Vector3 currentVelocity;
    public EFacingDirection facingDirection = EFacingDirection.Right;

    [Header("Movement Settings")]
    public float accel = 10;
    public float maxSpeed = 10;
    public float frictionCoef = 0.5f;
    public float rotationSpeed = 0.25f;
    public float jumpForce = 50;

    [Space(20)]
    [Header("Floating Settings")]
    public EDoesFloat doesFloat = EDoesFloat.Floats;
    public float floatOffsetY = 1.53f;
    public float floatSpeed = 8;
    public float floatRadius = 1.5f;

    private float floorCheckSphereRadius = 0f;

    [Header("Booyah")]
    public GameObject TempSphere;

    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();

        LeftPlane = transform.Find("LeftPlane").gameObject;
        RightPlane = transform.Find("RightPlane").gameObject;

        floorCheckSphereRadius = capsule.radius;
        //transform.position += new Vector3(0, 10, 0);

        Game.GInstance.SetPlayerActor(this);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        ApplyVelocity(dt);
        UpdateFacing(dt);

        if (doesFloat == EDoesFloat.Floats)
        {
            Vector3 offset = LeftPlane.transform.localPosition;
            offset.y = Mathf.Sin(Time.time / floatSpeed) * floatRadius + floatOffsetY;
            //print(y);
            LeftPlane.transform.localPosition = offset;
            RightPlane.transform.localPosition = offset;
        }

    }

    // Queues up movement for the next movement update, preferably a unit vector
    public void AddMovementInput(Vector3 Direction, float Scale)
    {
        pendingMovement += Direction * Scale;
    }

    public void UpdateFacing(float dt)
    {
        float yaw = transform.rotation.eulerAngles.y;
        if (currentVelocity.magnitude > 0)
        {

            facingDirection = currentVelocity.x < 0 ? EFacingDirection.Left : EFacingDirection.Right;
        }

        float targetYaw = (facingDirection == EFacingDirection.Left) ? -180 : 0;

        //if(facingDirection == EFacingDirection.Left)
        //{

        //    yaw -= rotationSpeed * dt;
        //    targetYaw = -180f;
        //}
        //else
        //{
        //    yaw += rotationSpeed * dt;
        //    targetYaw = 0f;
        //}

        yaw = Mathf.LerpAngle(yaw, targetYaw, rotationSpeed);
        if(Mathf.Approximately(yaw, targetYaw))
        {
            yaw = targetYaw;
        }


       //if(yaw > -180 && yaw < 0)
       // { 
            //if (facingDirection == EFacingDirection.Left)
            //{
            //    yaw = Mathf.Max(yaw - RotationSpeed * dt, -180);
            //}
            //else
            //{
            //    yaw = Mathf.Min(yaw + RotationSpeed * dt, 0);
            //}
            transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        //}
    }

    public void ApplyVelocity(float dt)
    {
        if(Mathf.Approximately(pendingMovement.magnitude, 0))
        {
            currentVelocity *= frictionCoef;
            if (currentVelocity.magnitude < 0.01)
            {
                currentVelocity = Vector3.zero;
            }
        }
        else
        {
            currentVelocity += pendingMovement * accel * dt;
        }
        
        float mag = currentVelocity.magnitude;

        Vector3 direction = currentVelocity.normalized;
        if (mag > maxSpeed)
        {
            currentVelocity = direction * maxSpeed;

        }
       
        body.MovePosition(transform.position + currentVelocity * dt);

        

        pendingMovement = Vector3.zero;
    }

    public void TryJump()
    {
        Vector3 chkPos = transform.position;
        chkPos.y -= capsule.bounds.extents.y;
        Debug.Log(Physics.CheckSphere(chkPos, floorCheckSphereRadius, ~(1 << 8)));
        //Instantiate(TempSphere, chkPos, new Quaternion());
        if (Physics.CheckSphere(chkPos, floorCheckSphereRadius, ~(1 << 8)) && body.velocity.y <= 0)
        {
            Vector3 oldVel = body.velocity;
            oldVel.y = 0;
            body.velocity = oldVel;
            body.AddForce(Vector3.up * jumpForce);
        }
    }

    public void SetState(EActorState NewState)
    {
        CurrentState = NewState;
    }

    public EActorState GetState()
    {
        return CurrentState;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Transform GetInteractibleTargetTransform()
    {
        return transform;
    }
}
