using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnEscape : MonoBehaviour {

	void Update() {
		if (transform.position.y < -200){
			Debug.Log("Game Over.");
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
			 Application.OpenURL(webplayerQuitURL);
#else
			 Application.Quit();
#endif
		}
	}
}
