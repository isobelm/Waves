using UnityEngine;

[System.Serializable]
public class SoundEffectSound
{
    public enum SoundName
    {
        WIN,
        DEAD
    }

    public SoundName name;
    public AudioClip audioClip;
}