using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum SceneName
    {
        MAIN_MENU,
        LEVEL_ONE
    }

    [SerializeField] private Sound[] musicSounds, audioSounds;
    [SerializeField] private AudioSource musicAudioSource, soundEffectSource;
    [SerializeField] private SceneName sceneName;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(Sound.AudioName name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        musicAudioSource.Stop();
        musicAudioSource.loop = true;
        musicAudioSource.clip = sound.audioClip;
        musicAudioSource.Play();
    }

    private void Start()
    {
        if (sceneName == SceneName.MAIN_MENU)
        {
            PlayMusic(Sound.AudioName.MAIN_MENU_MUSIC);
        }
        else if ( sceneName == SceneName.LEVEL_ONE)
        {
            PlayMusic(Sound.AudioName.LEVEL_ONE_MUSIC);
        }
    }
}
