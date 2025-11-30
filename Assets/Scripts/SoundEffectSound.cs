using UnityEngine;

[System.Serializable]
public class SoundEffectSound
{
    public enum SoundName
    {
        WIN,
        DEAD,
        CURSOR_CANCEL,
        CURSOR_CONFIRM
    }

    public SoundName name;
    public AudioClip audioClip;
}