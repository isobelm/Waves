using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] musicSounds, audioSounds;
    [SerializeField] private AudioSource musicAudioSource, soundEffectSource;
    
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

        musicAudioSource.loop = true;
        musicAudioSource.clip = sound.audioClip;
        musicAudioSource.Play();
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            PlayMusic(Sound.AudioName.MAIN_MENU_MUSIC);
        }
        else
        {
            PlayMusic(Sound.AudioName.LEVEL_ONE_MUSIC);
        }
    }
}
