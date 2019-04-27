using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState
{
    Walking,
    ReceivingItem,
    InConversation
}

public class PlayerController : MonoBehaviour
{
    public InputWrapper Controls;

    private EPlayerState CurrentState = EPlayerState.Walking;

    public Actor PlayerActor;
    //public Actor PlayerActor
    //{
    //    get;

    //    set;
    //}

    // Start is called before the first frame update
    void Start()
    {
        Controls = new InputWrapper();

        Game.GInstance.SetPlayerController(this);
    }

    // Update is called once per frame
    void Update()
    {

        // Poll input
        Vector3 Move = Controls.GetXY();
        PlayerActor.AddMovementInput(Move, 1f);

    }

    public void SetState(EPlayerState NewState)
    {
        CurrentState = NewState;
    }

    public EPlayerState GetState()
    {
        return CurrentState;
    }
}
