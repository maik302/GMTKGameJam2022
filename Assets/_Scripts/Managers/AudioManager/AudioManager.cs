using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private float _fadeScaledRate = .1f;

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
            sound.isPlaying = false;
		}
    }

	public void Play(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            // Stop any background music that is playing
            if (sound.isBGM) {
                Sound playingBGM = Array.Find(Sounds, s => s.isBGM && s.isPlaying);
                if (playingBGM != null) {
                    Stop(playingBGM.name);
                }
            }

            sound.audioSource.Play();
            sound.isPlaying = true;
        }
	}

    public void Stop(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            sound.audioSource.Stop();
            sound.isPlaying = false;
        }
    }
}
