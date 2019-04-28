using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour {
	public GameObject dialogManagerUI;
	private DialogManagerUI _dialogManagerUI;
	public GameObject player;
	public InputWrapper controls;
	public Dictionary<string, string> gameVars = new Dictionary<string, string>();

	public Dialog initDialog;

	private IDialogItem currentItem;

	private void Start() {
		if (player == null) {
			Debug.LogError("Player has not been set on the Dialog Manager.");
		}
		if (dialogManagerUI == null) {
			Debug.LogError("dialogManagerUI has not been set on the Dialog Manager.");
		} else {
			_dialogManagerUI = dialogManagerUI.GetComponent<DialogManagerUI>();
			Debug.Assert(_dialogManagerUI, "Cannot find DialogManagerUI component.");
		}
		RunInitDialog();
	}

	public void RunInitDialog() {
		if (initDialog != null) {
			setDialog(initDialog.getRoot());
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
		}
    }

	public void setDialog(IDialogItem item) {
        Debug.Assert(currentItem == null);
        Debug.Assert(player != null && player.GetComponent<Actor>());
        Debug.Log("setDialog " + item);
        player.GetComponent<Actor>().SetState(EActorState.InConversation); // can't move
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

