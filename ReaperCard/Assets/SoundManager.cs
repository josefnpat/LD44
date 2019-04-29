using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip sound;
    }
    public List<SoundEntry> sounds = new List<SoundEntry>();
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Play(string soundName)
    {
        soundName = soundName.Trim();
        var sound = sounds.Find(item => item.name == soundName);
        if(sound != null)
            audioSource.PlayOneShot(sound.sound);
        else
            Debug.LogWarning("Unknown sound with name " + soundName);
    }
}
