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
	public float fadeOutTime = 0.8f;
	public float fadeInTime = 0.8f;

	private AudioSource audioSource;
	private MusicEntry currentSong;
	private MusicEntry fadeIntoSong;

	void Start() {
		audioSource = gameObject.GetComponent<AudioSource>();
		ChangeToInit();
	}

	public void ChangeToInit() {
		ChangeTo(initFile);
	}

	public void ChangeTo(string songName) {
		songName = songName.Trim();
		var song = songs.Find(item => item.name == songName);

		if (song != null && song == currentSong) {
			Debug.Log("Play current song -> abort!");
			return;
		}

		if(songName.Length == 0) {
			Debug.Log("Stopping music.");
			audioSource.Stop();
			currentSong = null;
			return;
		}

		Debug.Assert(song != null, "Unknown song with name " + songName);
		Debug.Log("Changing music to: " + songName);
		if(currentSong == null) {
			Debug.Log("play new song");
			audioSource.clip = song.song;
			audioSource.Play();
			currentSong = song;
		} else {
			Debug.Log("Set fadeinto");
			fadeIntoSong = song;
		}
	}

	void Update() {
		if(currentSong != null) {
			if(fadeIntoSong == null) {
				if(currentSong != null) {
					var volume = audioSource.volume + Time.deltaTime / fadeInTime;
					audioSource.volume = Mathf.Min(1f, volume);
				}
			} else {
				var volume = audioSource.volume - Time.deltaTime / fadeOutTime;
				audioSource.volume = Mathf.Max(0f, volume);
				if(volume < 0) {
					audioSource.Stop();
					audioSource.clip = fadeIntoSong.song;
					audioSource.Play();
					currentSong = fadeIntoSong;
					fadeIntoSong = null;
				}
			}
		}
	}
}
