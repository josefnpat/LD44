using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class PlayerController : MonoBehaviour
{
    public InputWrapper Controls = new InputWrapper();

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Controls.GetXY();
        var actor = GetComponent<Actor>();
        actor.AddMovementInput(move, 1f);

        if (Controls.IsDown(EKey.Jump))
        {
            actor.TryJump();
        }
    }
}
