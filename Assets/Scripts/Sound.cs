using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum AudioName
    {
        MAIN_MENU_MUSIC,
        LEVEL_ONE_MUSIC
    }

    public AudioName name;
    public AudioClip audioClip;
}