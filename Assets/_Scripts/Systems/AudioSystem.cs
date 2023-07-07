using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem> {
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundsSource;

    [SerializeField] private List<AudioClip> soundFX;

    public void PlayMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
        soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        soundsSource.PlayOneShot(clip, vol);
    }
    
    public void PlaySound(string key, float vol = 1) {
        soundsSource.PlayOneShot(soundFX.Find(fx => fx.name.Equals(key)));
    }
}