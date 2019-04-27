using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public InputWrapper Controls;

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
        if(!PlayerActor)
        {
            return;
        }

        // Poll input
        Vector3 Move = Controls.GetXY();
        PlayerActor.AddMovementInput(Move, 1f);
        
        if(Controls.IsDown(EKey.Jump))
        {
            PlayerActor.TryJump();
        }
    }


}
