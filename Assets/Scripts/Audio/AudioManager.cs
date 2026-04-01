using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    public Audio[] AudioObjects;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        foreach (Audio audio in AudioObjects)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.Clip;
            audio.source.volume = audio.Volume;
            audio.source.pitch = audio.Pitch;
            audio.source.loop = audio.Loop;
        }
    }

    public void Play(string audioName)
    {
        Debug.Log($"[Audio_Manager] Playing: {audioName}.");
        Audio audio = Array.Find(AudioObjects, audio => audio.Name == audioName);
        if (audio == null)
        {
            Debug.LogWarning($"[Audio_Manager] Audio: {audioName} not found.");
            return;
        }
        audio.source.Play();
    }
}
