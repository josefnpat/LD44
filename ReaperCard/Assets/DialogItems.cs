using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IDialogItem {
    void enter(DialogManager dialogManager);
    // since next() decides when to leave the state,
    // put code that would have been in exit() before into next()!
    IDialogItem next(DialogManager dialogManager);
};

public class DialogBranch : IDialogItem {
    private string varName;
    private Dictionary<string, IDialogItem> branches;
    private IDialogItem defaultBranch;

    public DialogBranch(string varName) {
        this.varName = varName;
    }

    public void setBranches(Dictionary<string, IDialogItem> branches, IDialogItem defaultBranch) {
        this.branches = branches;
        this.defaultBranch = defaultBranch;
    }

    public void enter(DialogManager dialogManager) {
    }

    public IDialogItem next(DialogManager dialogManager) {
        if(varName.StartsWith("item:")) {
            Debug.Assert(branches.ContainsKey("true"));
            var itemName = varName.Substring("item:".Length);
            var inventory = dialogManager.player.GetComponent<Inventory>();
            if(inventory.hasItem(itemName)) {
                return branches["true"];
            } else {
                return defaultBranch;
            }
        } else {
            var gameVars = dialogManager.gameVars;
            if(gameVars.ContainsKey(varName) && branches.ContainsKey(gameVars[varName])) {
                return branches[gameVars[varName]];
            } else {
                return defaultBranch;
            }
        }
    }

    public override string ToString() {
        return string.Format("DialogBranch(variable='{0}', branches=[{1}])", varName, string.Join(", ", branches.Keys));
    }
};

public class DialogChoice : IDialogItem {
    public class Choice {
        public string text;
        public IDialogItem next;

        public Choice(string text, IDialogItem next) {
            this.text = text;
            this.next = next;
        }
    };

    private List<Choice> choices;

    public void setChoices(List<Choice> choices) {
        this.choices = choices;
    }

    public void enter(DialogManager dialogManager) {
        var dialogManagerUI = dialogManager.GetComponent<DialogManagerUI>();
        dialogManagerUI.SetOptions(choices.Select(choice => choice.text).ToList());
    }

    public IDialogItem next(DialogManager dialogManager) {
        var dialogManagerUI = dialogManager.GetComponent<DialogManagerUI>();
        if(dialogManagerUI.ReadyForNext()) {
            var choice = choices[dialogManagerUI.GetCurrentSelectedOption()];
            Debug.Log("dialog - chosen: " + choice.text);
            return choice.next;
        }
        return this;
    }

    public override string ToString() {
        var choiceText = new List<string>();
        foreach(var choice in choices)
            choiceText.Add("'" + choice.text + "'");
        return string.Format("DialogChoice(choices=[{0}])", string.Join(", ", choiceText));
    }
};

public class DialogSetVar : IDialogItem {
    private string variable;
    private string value;
    private IDialogItem nextItem;

    public DialogSetVar(string variable, string value) {
        this.variable = variable;
        this.value = value;
    }

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }

    public void enter(DialogManager dialogManager) {
        dialogManager.gameVars[variable] = value;
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }

    public override string ToString() {
        return string.Format("DialogSetVar(variable='{0}', value='{1}', next='{2}')", variable, value, Util.typeName(nextItem));
    }
};

public class DialogText : IDialogItem {
    private string text;
    private IDialogItem nextItem;

    public DialogText(string text) {
        this.text = text;
    }

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }

    public void enter(DialogManager dialogManager) {
        var dialogManagerUI = dialogManager.GetComponent<DialogManagerUI>();
		dialogManagerUI.SetText(text, dialogManager.npc.name);
	}

    public IDialogItem next(DialogManager dialogManager) {
        var dialogManagerUI = dialogManager.GetComponent<DialogManagerUI>();
		if(dialogManagerUI.ReadyForNext()) {
            return nextItem;
        }
		return this;
    }

    public override string ToString() {
        return string.Format("DialogText(text='{0}', next={1})", text, Util.typeName(nextItem));
    }
};

public class DialogGiveItem : IDialogItem {
    private string itemName;
    private IDialogItem nextItem;

    public DialogGiveItem(string itemName) {
        this.itemName = itemName;
    }

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }

    public void enter(DialogManager dialogManager) {
        var inventory = dialogManager.player.GetComponent<Inventory>();
        inventory.addItem(itemName);
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }

    public override string ToString() {
        return string.Format("DialogGiveItem(item='{0}', next={1})", itemName, Util.typeName(nextItem));
    }
};

public class DialogTakeItem : IDialogItem {
    private string itemName;
    private IDialogItem nextItem;

    public DialogTakeItem(string itemName) {
        this.itemName = itemName;
    }

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }
    public void enter(DialogManager dialogManager) {
        var inventory = dialogManager.player.GetComponent<Inventory>();
        inventory.removeItem(itemName);
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }

    public override string ToString() {
        return string.Format("DialogTakeItem(item='{0}', next={1})", itemName, Util.typeName(nextItem));
    }
};

public class DialogAudio : IDialogItem {
    private string soundFilename;
    private IDialogItem nextItem;

    public DialogAudio(string soundFilename) {
        this.soundFilename = soundFilename;
    }

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }

    public void enter(DialogManager dialogManager) {
        // TODO: play sound
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }

    public override string ToString() {
        return string.Format("DialogAudio(filename='{0}', next={1})", soundFilename, Util.typeName(nextItem));
    }
};

public class DialogDie : IDialogItem {
    public IDialogItem nextItem;

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }

    public void enter(DialogManager dialogManager) {
        // TODO: tell dialogManager.npc to die
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }

    public override string ToString() {
        return string.Format("DialogDie(next={0})", Util.typeName(nextItem));
    }
};
