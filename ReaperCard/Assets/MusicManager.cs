using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	[System.Serializable]
	public class MusicEntry {
		public string name;
		public AudioClip song;
	}
	public List<MusicEntry> songs = new List<MusicEntry>();
	public string initFile;
	public string currentFile;
	private AudioSource audioSource;

	void Start() {
		audioSource = gameObject.GetComponent<AudioSource>();
		ChangeToInit();
	}

	public void ChangeToInit() {
		ChangeTo(initFile);
	}

	public void ChangeTo(string songName) {
		if (songName == currentFile) {
			return;
		}
		currentFile = songName;
		songName = songName.Trim();
		Debug.Log("Changing music to: " + songName);
		audioSource.Stop();
		if(songName.Length > 0) {
			var song = songs.Find(item => item.name == songName);
			Debug.Assert(song != null, "Unknown song with name " + songName);
			audioSource.clip = song.song;
			Debug.Log("audioSource.clip:" + audioSource.clip);
			audioSource.Play();
		}
	}
}
