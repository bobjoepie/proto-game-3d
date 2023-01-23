using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioSource EffectsSource;
    public AudioSource MusicSource;
    public AudioClip music;
    public List<AudioClip> soundQueue = new List<AudioClip>();
    public int maxSimultaneousSounds = 5;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;
    public static AudioManager Instance { get; private set; }

    private AudioManager()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayMusic(music);
    }

    private void Update()
    {
        PlayQueue();
    }

    public void Play(AudioClip clip)
    {
        if (soundQueue.Count < maxSimultaneousSounds)
        {
            soundQueue.Add(clip);
        }
    }

    private void PlayQueue()
    {
        foreach (var clip in soundQueue)
        {
            EffectsSource.clip = clip;
            EffectsSource.Play();
        }

        soundQueue.Clear();
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }
}