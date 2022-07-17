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
		}
    }

	public void Play(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            sound.audioSource.Play();
        }
	}

    public void Stop(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            sound.audioSource.Stop();
        }
    }

    public void PlayWithFadeIn(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            PlayWithFade(sound);
        }
    }

    private IEnumerator PlayWithFade(Sound sound) {
        sound.audioSource.volume = 0;
        sound.audioSource.Play();

        while (sound.audioSource.volume < sound.volume) {
            sound.audioSource.volume += _fadeScaledRate * Time.deltaTime;

            yield return null;
        }
    }

    public void StopWithFadeOut(string name) {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound != null) {
            StopWithFade(sound);
        }
    }

    private IEnumerator StopWithFade(Sound sound) {
        while (sound.audioSource.volume > 0) {
            sound.audioSource.volume -= _fadeScaledRate * Time.deltaTime;

            yield return null;
        }
        sound.audioSource.Stop();
    }
}
