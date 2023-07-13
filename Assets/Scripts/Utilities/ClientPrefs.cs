using UnityEngine;

public static class ClientPrefs
{
    private const string MusicToggleKey = "MusicToggle";
    private const string SoundEffectsToggleKey = "SoundEffectsToggle";

    public static void Initialize()
    {
        if (!PlayerPrefs.HasKey(MusicToggleKey))
        {
            PlayerPrefs.SetInt(MusicToggleKey, 1);
        }

        if (!PlayerPrefs.HasKey(SoundEffectsToggleKey))
        {
            PlayerPrefs.SetInt(SoundEffectsToggleKey, 1);
        }
    }

    public static bool GetMusicToggle()
    {
        return PlayerPrefs.GetInt(MusicToggleKey, 1) == 1;
    }

    public static void SetMusicToggle(bool toggle)
    {
        PlayerPrefs.SetInt(MusicToggleKey, toggle ? 1 : 0);
    }

    public static bool GetSoundEffectsToggle()
    {
        return PlayerPrefs.GetInt(SoundEffectsToggleKey, 1) == 1;
    }

    public static void SetSoundEffectsToggle(bool toggle)
    {
        PlayerPrefs.SetInt(SoundEffectsToggleKey, toggle ? 1 : 0);
    }

    public static void ResetClientPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public static bool GetTutorialCompleted()
    {
        return PlayerPrefs.GetInt("TutorialCompleted", 0) == 1;
    }

    public static void SetTutorialCompleted(bool completed)
    {
        PlayerPrefs.SetInt("TutorialCompleted", completed ? 1 : 0);
    }
}
