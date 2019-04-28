using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip sound;
    }
    public List<SoundEntry> sounds = new List<SoundEntry>();
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Play(string soundName)
    {
        soundName = soundName.Trim();
        var sound = sounds.Find(item => item.name == soundName);
        Debug.Assert(sound != null, "Unknown sound with name " + soundName);
        audioSource.PlayOneShot(sound.sound);
    }
}
