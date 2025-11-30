using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum SceneName
    {
        MAIN_MENU,
        LEVEL_ONE,
        WIN,
        DEAD
    }

    [SerializeField] private MusicSound[] musicSounds;
    [SerializeField] private SoundEffectSound[] soundEffectSounds;
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

    public void PlayMusic(MusicSound.MusicName name)
    {
        MusicSound sound = Array.Find(musicSounds, x => x.name == name);

        musicAudioSource.Stop();
        musicAudioSource.loop = true;
        musicAudioSource.clip = sound.audioClip;
        musicAudioSource.Play();
    }

    public void PlaySound(SoundEffectSound.SoundName name)
    {
        SoundEffectSound sound = Array.Find(soundEffectSounds, x => x.name == name);

        soundEffectSource.PlayOneShot(sound.audioClip);
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    private void Start()
    {
        PlayMusic(MusicSound.MusicName.MAIN_MENU_MUSIC);
    }
}
