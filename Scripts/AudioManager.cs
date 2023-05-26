using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    [HideInInspector] public float currentVolume;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        currentVolume = PlayerPrefs.GetFloat("globalVolume");

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * currentVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        PlaySound("bgMusic");
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);       
        if (s == null) return;                                                      //eðer o isimde bir ses dosyasý yoksa return et
        SetSound(name, currentVolume);
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);                  
        if (s == null) return;                                                      
        s.source.Stop();
    }
    public void SetSound(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.volume = volume * s.volume;
    }

    public void SetGV(float volume)                     //Level select menu'deki SetGlobalVolume() fontksiyonu için yazýlýd
    {
        PlaySound("slider");
        currentVolume = volume;
        SetSound("bgMusic", volume);
    }
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}