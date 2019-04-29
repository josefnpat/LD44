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

public enum EActorState
{
    Walking,
    ReceivingItem,
    InConversation
}

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Actor : MonoBehaviour
{
    // References to own components
    CapsuleCollider capsule;
    Rigidbody body;
    public GameObject LeftPlane;
    public GameObject RightPlane;
    public GameObject PlaneContainer;

    // State vars
    private EActorState CurrentState = EActorState.Walking;
    private Vector3 pendingMovement = new Vector3();
    public Vector3 currentVelocity;
    public EFacingDirection facingDirection = EFacingDirection.Right;


    // Config settings
    [Header("Movement Settings")]
    public float accel = 10f;
    public float maxSpeed = 10f;
    public float frictionCoef = 0.5f;
    public float rotationSpeed = 0.25f;
    public float jumpForce = 50f;

    private float floorCheckSphereRadius = 0f;

    [Header("Booyah")]
    public GameObject TempSphere;

    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();

        PlaneContainer = transform.Find("PlaneContainer").gameObject;
        LeftPlane = PlaneContainer.transform.Find("LeftPlane").gameObject;
        RightPlane = PlaneContainer.transform.Find("RightPlane").gameObject;


        floorCheckSphereRadius = capsule.radius;

        //transform.position += new Vector3(0, 10, 0);

        //Game.GInstance.SetPlayerActor(this);
    }

   public bool jumpHax = true;

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        ApplyVelocity(dt);
        UpdateFacing(dt);

        if(jumpHax)
        {
            if(Random.value > 0.9)
            {
                TryJump();
            }
        }
    }

    // Queues up movement for the next movement update, preferably a unit vector
    public void AddMovementInput(Vector3 Direction, float Scale)
    {
        pendingMovement += Direction * Scale;
    }

    public void UpdateFacing(float dt)
    {
        float yaw = PlaneContainer.transform.localRotation.eulerAngles.y;
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
            PlaneContainer.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        //}
    }

    public void ApplyVelocity(float dt)
    {
        if (CurrentState == EActorState.InConversation)
            return;

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
        if (CurrentState == EActorState.InConversation)
            return;

        Vector3 chkPos = transform.position;
        chkPos.y -= capsule.bounds.extents.y;
        //Instantiate(TempSphere, chkPos, new Quaternion());
        if (Physics.CheckSphere(chkPos, floorCheckSphereRadius, ~(1 << 8)) && body.velocity.y <= 0)
        {
            Vector3 oldVel = body.velocity;
            oldVel.y = 0;
            body.velocity = oldVel;
            body.AddForce(Vector3.up * jumpForce);
            SoundManager.instance.Play("jump");
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
