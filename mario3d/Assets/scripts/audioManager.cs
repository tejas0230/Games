using UnityEngine.Audio;
using System;
using UnityEngine;

public class audioManager : MonoBehaviour
{

    private static audioManager _instance;

    public static audioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<audioManager>(); 
            }
            return _instance;
        }
    }
    public sound[] sounds;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach(sound s in sounds)
        {
            s.audioSrc = gameObject.AddComponent<AudioSource>();
            s.audioSrc.clip = s.clip;
            s.audioSrc.volume = s.volume;
            s.audioSrc.pitch = s.pitch;
            s.audioSrc.loop = s.loop;
        }
    }

    public void PlaySound(string name)
    {
        sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;        
        s.audioSrc.Play();
            
    }

    
}
