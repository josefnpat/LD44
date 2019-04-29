using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MusicManager))]
public class MusicManagerEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		MusicManager myScript = (MusicManager) target;

		if (GUILayout.Button("Change To Init")) {
			myScript.ChangeToInit();
		}

	}
}