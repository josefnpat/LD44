using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {
    public GameObject player;
    public InputWrapper controls;
    public Dictionary<string, string> gameVars = new Dictionary<string, string>();

    private IDialogItem currentItem;

    public void setDialog(IDialogItem item) {
        if (currentItem == null)
        {
            player.GetComponent<Actor>().SetState(EActorState.InConversation); // can't move
            currentItem = item;
        }

        var next = currentItem.next(this);
        if(next != null && next != currentItem)
        {
            currentItem.exit(this);
            next.enter(this);
            currentItem = next;
            next = currentItem.next(this);
        }
        else
        {
            currentItem.exit(this);
            currentItem = null;
            player.GetComponent<Actor>().SetState(EActorState.Walking);
        }
    }
};

