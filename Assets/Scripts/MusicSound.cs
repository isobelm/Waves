using UnityEngine;

[System.Serializable]
public class MusicSound
{
    public enum MusicName
    {
        MAIN_MENU_MUSIC,
        LEVEL_ONE_MUSIC
    }

    public MusicName name;
    public AudioClip audioClip;
}