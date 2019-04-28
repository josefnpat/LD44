using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;

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

    IDialogItem parseSfxNode(JSONNode jsonData) {
        string name = jsonData["name"];
        var item = cacheItem(jsonData["id"], new DialogSfx(name.Substring("sfx:".Length)));
        item.setNext(getNode(jsonData["next"]));
        return item;
    }

    IDialogItem parseMusicNode(JSONNode jsonData) {
        string name = jsonData["name"];
        var item = cacheItem(jsonData["id"], new DialogMusic(name.Substring("music:".Length)));
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
                } else if(name.StartsWith("sfx:")) {
                    return parseSfxNode(jsonData);
                } else if(name.StartsWith("music:")) {
                    return parseMusicNode(jsonData);
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

    private string nodeSummary(JSONNode jsonData) {
        string type = jsonData["type"];
        switch(type) {
            case "Branch":
                return string.Format("Branch(variable='{0}')", jsonData["variable"]);
            case "Choice":
                return string.Format("Choice('{0}')", jsonData["name"]);
            case "Set":
                return string.Format("Set(variable='{0}', value='{1}')", jsonData["variable"], jsonData["value"]);
            case "Text":
                return string.Format("Text('{0}')", jsonData["name"]);
            case "Node":
                return string.Format("Node('{0}')", jsonData["name"]);
            default:
                return string.Format("Unknown Node (id: '{}')", jsonData["id"]);
        }
    }

    void OnEnable() {
        // Obviously, none of this is efficient at all
        var jsonData = JSON.Parse(jsonFile.text);
        var possibleEntryNodes = new HashSet<string>();
        for(var i = 0; i < jsonData.Count; ++i) {
            var node = jsonData[i];
            var id = node["id"];
            nodeByIdCache[id] = node;
            possibleEntryNodes.Add(id);
        }

        for(var i = 0; i < jsonData.Count; ++i) {
            var node = jsonData[i];
            if(node["choices"].Count > 0) {
                for(var c = 0; c < node["choices"].Count; ++c) {
                    possibleEntryNodes.Remove(node["choices"][c]);
                }
            } else if(node["branches"].Count > 0) {
                foreach(var branch in node["branches"].Keys) {
                    possibleEntryNodes.Remove(node["branches"][branch]);
                }
            } else {
                string next = node["next"];
                if(next != null) {
                    possibleEntryNodes.Remove(next);
                }
            }
        }
        Debug.Assert(possibleEntryNodes.Count == 1,
            "Found multiple possible entry nodes (no input) for dialog " +
            dialogName + ": [" + string.Join(", ", possibleEntryNodes
                .Select(node => nodeSummary(nodeByIdCache[node]))) + "]");
        var entryId = possibleEntryNodes.ToList()[0];
        root = getNode(entryId);
    }

    public IDialogItem getRoot() {
        return root;
    }
}
