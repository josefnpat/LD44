using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {
    public GameObject player;
    public InputWrapper controls;
    public Dictionary<string, string> gameVars = new Dictionary<string, string>();

    private IDialogItem currentItem;

    public void setDialog(IDialogItem item) {
        player.GetComponent<Actor>().SetState(EActorState.InConversation); // can't move
        currentItem = item;
    }

    void Update() {
        if(currentItem != null) {
            var next = currentItem.next(this);
            while(next != null) {
                currentItem.exit(this);
                next.enter(this);

                if(next.GetType() == typeof(DialogEnd)) {
                    currentItem = null;
                    player.GetComponent<Actor>().SetState(EActorState.Walking);
                    break;
                }
                next = currentItem.next(this);
            };
        }
    }
};

