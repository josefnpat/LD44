using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public string initFile;
	private AudioSource audioSource;

	void Start() {
		audioSource = gameObject.GetComponent<AudioSource>();
		ChangeToInit();
	}

	public void ChangeToInit() {
		ChangeTo(initFile);
	}

	public void ChangeTo(string song) {
		Debug.Log("Changing music to: " + song);
		audioSource.Stop();
		audioSource.clip = Resources.Load("Music/"+song) as AudioClip;
		//audioSource.clip = Resources.Load<AudioClip>(song);
		Debug.Log("audioSource.clip:" + audioSource.clip);
		audioSource.Play();
	}

}
