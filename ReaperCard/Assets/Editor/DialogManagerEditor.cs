using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DialogManager))]
public class DialogManagerEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		DialogManager myScript = (DialogManager) target;

		if (GUILayout.Button("Run Init Dialog")) {
			myScript.RunInitDialog();
		}

	}
}