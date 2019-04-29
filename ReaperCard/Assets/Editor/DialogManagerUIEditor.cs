using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DialogManagerUI))]
public class DialogManagerUIEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		DialogManagerUI myScript = (DialogManagerUI) target;

		if (GUILayout.Button("Example Set Options")) {
			myScript.ExampleSetOptions();
		}

		if (GUILayout.Button("Example Set Text")) {
			myScript.ExampleSetText();
		}

		if (GUILayout.Button("Example Set Event")) {
			myScript.ExampleSetEvent();
		}

	}
}