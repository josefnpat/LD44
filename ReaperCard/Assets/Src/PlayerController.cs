using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public InputWrapper Controls;

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
    }
}
