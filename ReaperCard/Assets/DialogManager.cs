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
			Debug.LogWarning("Player has not been set on the Dialog Manager.");
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

	public void Update() {
		if(currentItem.next(this) != null && _dialogManagerUI.ReadyForNext()){
			setDialog(currentItem.next(this));
		}
	}

	public void setDialog(IDialogItem item) {
		if (currentItem == null)
		{
			if (player != null) {
				player.GetComponent<Actor>().SetState(EActorState.InConversation); // can't move
			}
			currentItem = item;
			item.enter(this, _dialogManagerUI);
		}

		var next = currentItem.next(this);
		if(next != null && next != currentItem)
		{
			currentItem.exit(this);
			next.enter(this, _dialogManagerUI);
			currentItem = next;
			next = currentItem.next(this);
		}
		else
		{
			currentItem.exit(this);
			currentItem = null;
			if (player != null) {
				player.GetComponent<Actor>().SetState(EActorState.Walking);
			}
		}
	}
};

