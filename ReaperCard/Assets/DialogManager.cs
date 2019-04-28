using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StartDialogEvent : UnityEvent<GameObject, GameObject> {
}

public class DialogManager : MonoBehaviour {
	public GameObject dialogManagerUI;
	public GameObject initDialogPlayer;
	public InputWrapper controls;
	public Dictionary<string, string> gameVars = new Dictionary<string, string>();

     [HideInInspector]
    public GameObject player;
     [HideInInspector]
    public GameObject npc;

	public Dialog initDialog;

    public StartDialogEvent startDialog;
    public UnityEvent endDialog;

	private IDialogItem currentItem;

	private void Start() {
		if (dialogManagerUI == null) {
			Debug.LogError("dialogManagerUI has not been set on the Dialog Manager.");
		}
		RunInitDialog();
	}

	public void RunInitDialog() {
		if (initDialog != null) {
            Debug.Assert(initDialogPlayer != null);
			setDialog(initDialog.getRoot(), initDialogPlayer, null);
		}
	}

    void advanceDialog() {
        Debug.Assert(currentItem != null);
		var next = currentItem.next(this);
		while(next != null && next != currentItem) {
            Debug.Log("advance dialog " + next);
			next.enter(this);
			currentItem = next;
			next = currentItem.next(this);
		};

        if(next == null) {
            Debug.Log("end conversation");
			currentItem = null;
            Debug.Assert(player != null);
            player.GetComponent<Actor>().SetState(EActorState.Walking);
            endDialog.Invoke();
		}
    }

	public void setDialog(IDialogItem item, GameObject player, GameObject npc) {
        Debug.Assert(currentItem == null);
        Debug.Assert(player != null && player.GetComponent<Actor>());

        this.player = player;
        this.npc = npc;

        Debug.Log("setDialog " + item);
        player.GetComponent<Actor>().SetState(EActorState.InConversation); // can't move
        startDialog.Invoke(player, npc);

        currentItem = item;
		currentItem.enter(this);
        advanceDialog();
	}

	public void Update() {
		if(currentItem != null) {
			advanceDialog();
		}
	}
};

