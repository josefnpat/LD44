using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogItem {
    void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI);
    void exit(DialogManager dialogManager);
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

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog: branch on " + varName);
    }

    public void exit(DialogManager dialogManager) {
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

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        var choiceText = new List<string>();
        Debug.Log("dialog - choices: " + string.Join(", ", choiceText));
        // create ui items
    }

    public void exit(DialogManager dialogManager) {
        // destroy ui items
    }

    public IDialogItem next(DialogManager dialogManager) {
        var choice = choices[Random.Range(0, choices.Count)];
        Debug.Log("dialog - chosen: " + choice.text);
        return choice.next;
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

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - set var " + variable + " to " + value);
        dialogManager.gameVars[variable] = value;
    }

    public void exit(DialogManager dialogManager) {
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
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

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - show text: " + text);
        // spawn ui elements
    }

    public void exit(DialogManager dialogManager) {
        // remove ui elements
    }

    public IDialogItem next(DialogManager dialogManager) {
        //var controls = dialogManager.player.GetComponent<PlayerController>().Controls;
        //if(true || controls.IsDown(EKey.Confirm)) return this.nextItem;
        return this;
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

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - give item " + itemName);
        var inventory = dialogManager.player.GetComponent<Inventory>();
        inventory.addItem(itemName);
    }

    public void exit(DialogManager dialogManager) {
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
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
    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - take item " + itemName);
        var inventory = dialogManager.player.GetComponent<Inventory>();
        inventory.removeItem(itemName);
    }

    public void exit(DialogManager dialogManager) {
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
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

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - play audio " + soundFilename);
        // TODO: play sound
    }

    public void exit(DialogManager dialogManager) {
    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }
};

public class DialogDie : IDialogItem {
    public IDialogItem nextItem;

    public void setNext(IDialogItem nextItem) {
        this.nextItem = nextItem;
    }

    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - die");
        // TODO: have DialogManager.setDialog take a convo partner as an argument, then send it an event here?
    }

    public void exit(DialogManager dialogManager) {

    }

    public IDialogItem next(DialogManager dialogManager) {
        return nextItem;
    }
};

public class DialogEnd : IDialogItem {
    public void enter(DialogManager dialogManager, DialogManagerUI dialogManagerUI) {
        Debug.Log("dialog - end");
    }
    public void exit(DialogManager dialogManager) {}
    public IDialogItem next(DialogManager dialogManager) {
        return null;
    }
};
