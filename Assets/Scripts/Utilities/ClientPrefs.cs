using UnityEngine;

public static class ClientPrefs
{
    private const string MusicToggleKey = "MusicToggle";
    private const string SoundEffectsToggleKey = "SoundEffectsToggle";

    public static void Initialize()
    {
        try
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
        catch (PlayerPrefsException ex)
        {
            Debug.LogError("Error initializing client preferences: " + ex.Message);
        }
    }

    public static bool GetMusicToggle()
    {
        try
        {
            return PlayerPrefs.GetInt(MusicToggleKey, 0) == 1;
        }
        catch (PlayerPrefsException ex)
        {
            Debug.LogError("Error getting music toggle: " + ex.Message);
            return true; // Return a default value in case of error
        }
    }

    public static void SetMusicToggle(bool toggle)
    {
        try
        {
            PlayerPrefs.SetInt(MusicToggleKey, toggle ? 1 : 0);
        }
        catch (PlayerPrefsException ex)
        {
            Debug.LogError("Error setting music toggle: " + ex.Message);
        }
    }

    public static bool GetSoundEffectsToggle()
    {
        try
        {
            return PlayerPrefs.GetInt(SoundEffectsToggleKey, 0) == 1;
        }
        catch (PlayerPrefsException ex)
        {
            Debug.LogError("Error getting sound effects toggle: " + ex.Message);
            return true; // Return a default value in case of error
        }
    }

    public static void SetSoundEffectsToggle(bool toggle)
    {
        try
        {
            PlayerPrefs.SetInt(SoundEffectsToggleKey, toggle ? 1 : 0);
        }
        catch (PlayerPrefsException ex)
        {
            Debug.LogError("Error setting sound effects toggle: " + ex.Message);
        }
    }

    public static void ResetClientPrefs()
    {
        try
        {
            PlayerPrefs.DeleteAll();
        }
        catch (PlayerPrefsException ex)
        {
            Debug.LogError("Error resetting client preferences: " + ex.Message);
        }
    }
}
