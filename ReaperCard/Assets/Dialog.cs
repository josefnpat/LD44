using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject {
    public string dialogName;
    public TextAsset jsonFile;

    private IDialogItem root;
    private Dictionary<string, IDialogItem> itemById = new Dictionary<string, IDialogItem>();
    private Dictionary<string, JSONNode> nodeByIdCache = new Dictionary<string, JSONNode>();

    T cacheItem<T>(string id, T item) where T : IDialogItem {
        itemById[id] = item;
        return item;
    }

    IDialogItem parseBranchNode(JSONNode jsonData) {
        var item = cacheItem(jsonData["id"], new DialogBranch(jsonData["variable"]));

        var branches = new Dictionary<string, IDialogItem>();
        IDialogItem defaultBranch = null;
        foreach(var branch in jsonData["branches"].Keys) {
            if(branch == "_default") {
                defaultBranch = getNode(jsonData["branches"][branch]);
            } else {
                branches[branch] = getNode(jsonData["branches"][branch]);
            }
        }

        item.setBranches(branches, defaultBranch);
        return item;
    }

    IDialogItem parseSetNode(JSONNode jsonData) {
        var item = cacheItem(jsonData["id"], new DialogSetVar(jsonData["variable"], jsonData["value"]));
        item.setNext(getNode(jsonData["next"]));
        return item;
    }

    DialogChoice.Choice parseChoiceNode(JSONNode jsonData) {
        return new DialogChoice.Choice(jsonData["name"], getNode(jsonData["next"]));
    }

    IDialogItem parseTextNode(JSONNode jsonData) {
        string text = jsonData["name"];
        var item = cacheItem(jsonData["id"], new DialogText(text));
        if(jsonData["choices"].Count > 0) {
            var choices = new List<DialogChoice.Choice>();
            for(var i = 0; i < jsonData["choices"].Count; ++i) {
                choices.Add(parseChoiceNode(nodeByIdCache[jsonData["choices"][i]]));
            }
            var choiceItem = cacheItem(jsonData["id"], new DialogChoice());
            choiceItem.setChoices(choices);
            item.setNext(choiceItem);
        } else {
            item.setNext(getNode(jsonData["next"]));
        }
        return item;
    }

    IDialogItem parseGiveNode(JSONNode jsonData) {
        string name = jsonData["name"];
        var item = cacheItem(jsonData["id"], new DialogGiveItem(name.Substring("addinv:".Length)));
        item.setNext(getNode(jsonData["next"]));
        return item;
    }

    IDialogItem parseTakeNode(JSONNode jsonData) {
        string name = jsonData["name"];
        var item = cacheItem(jsonData["id"], new DialogTakeItem(name.Substring("removeinv:".Length)));
        item.setNext(getNode(jsonData["next"]));
        return item;
    }

    IDialogItem parseAudioNode(JSONNode jsonData) {
        string name = jsonData["name"];
        var item = cacheItem(jsonData["id"], new DialogAudio(name.Substring("audio:".Length)));
        item.setNext(getNode(jsonData["next"]));
        return item;
    }

    IDialogItem parseDieNode(JSONNode jsonData) {
        var item = cacheItem(jsonData["id"], new DialogDie());
        item.setNext(getNode(jsonData["next"]));
        return item;
    }

    IDialogItem getNode(string id) {
        if(id == null) return null;
        if(itemById.ContainsKey(id)) return itemById[id];
        var item = parseNode(nodeByIdCache[id]);
        Debug.Assert(itemById.ContainsKey(id));
        return item;
    }

    IDialogItem parseNode(JSONNode jsonData) {
        string type = jsonData["type"];
        switch(type) {
            case "Branch":
                return parseBranchNode(jsonData);
            case "Choice":
                Debug.LogError("Choice nodes should have been parsed by previous Text node!");
                return null;
            case "Set":
                return parseSetNode(jsonData);
            case "Text":
                return parseTextNode(jsonData);
            case "Node":
                string name = jsonData["name"];
                name = name.Trim().ToLower();
                if(name.StartsWith("addinv:")) {
                    return parseGiveNode(jsonData);
                } else if(name.StartsWith("removeinv:")) {
                    return parseTakeNode(jsonData);
                } else if(name.StartsWith("audio:")) {
                    return parseAudioNode(jsonData);
                } else if(name == "die") {
                    return parseDieNode(jsonData);
                } else if(name == "end") {
                    return null;
                } else {
                    Debug.LogError("Unrecognized node type 'Node' with name '" + jsonData["name"] + "' in " + dialogName);
                    return null;
                }
            default:
                Debug.LogError("Unrecognized node type '" + type + "'");
                return null;
        }
    }

    void OnEnable() {
        // Obviously, none of this is efficient at all
        var jsonData = JSON.Parse(jsonFile.text);
        JSONNode enter = null;
        for(var i = 0; i < jsonData.Count; ++i) {
            var node = jsonData[i];
            var id = node["id"];
            nodeByIdCache[id] = node;

            string type = node["type"];
            if(type == "Node") {
                string name = node["name"];
                name = name.Trim().ToLower();
                if(name == "enter") {
                    Debug.Assert(enter == null, "'enter' node has to be unique!");
                    enter = node;
                }
            }
        }
        if(enter == null) {
            // There is no enter node, the root node is the first in the file
            root = getNode(jsonData[0]["id"]);
        } else {
            root = getNode(enter["next"]);
        }
    }

    public IDialogItem getRoot() {
        return root;
    }
}
