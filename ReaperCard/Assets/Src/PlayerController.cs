using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public InputWrapper Controls;
    public PlayerComponent PlayerComp;

    private Actor PlayerActor;

    // Start is called before the first frame update
    void Start()
    {
        PlayerActor = gameObject.GetComponent<Actor>();
        Controls = new InputWrapper();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Move = Controls.GetXY();
        PlayerActor.AddMovementInput(Move, 1f);
        
        if(Controls.IsDown(EKey.Jump))
        {
            PlayerActor.TryJump();
        }

        if(Controls.IsDown(EKey.Confirm))
        {
            
            if(!PlayerComp)
            {
                PlayerComp = PlayerActor.gameObject.GetComponentInChildren<PlayerComponent>();
            }

            if(PlayerComp && PlayerComp.InteractObj)
            {
                print("confirm");
                Interactible interact = PlayerComp.InteractObj.GetComponent<Interactible>();
                interact.Interact.Invoke();
            }
        }
    }


}
