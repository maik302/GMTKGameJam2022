using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    // SINGLETON
    public static AudioManager Instance;

    public Sound[] Sounds;

    void Awake()
    {
        // Singleton configuration
        if (Instance == null) {
            Instance = this;
		} else {
            Destroy(gameObject);
            return;
		}

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in Sounds) {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
		}
    }

	public void play(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            sound.audioSource.Play();
        }
	}

    public void stop(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            sound.audioSource.Stop();
        }
    }
}
