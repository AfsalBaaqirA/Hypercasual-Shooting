using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    private static AudioManager instance;

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    // Audio mixer volume keys
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";

    // Default volume values
    private const float DEFAULT_MUSIC_VOLUME = 0.5f;
    private const float DEFAULT_SOUND_EFFECTS_VOLUME = 0.5f;

    // Reference to audio sources
    private AudioSource musicPlayer;
    private AudioSource soundEffectsPlayer;

    // Reference to audio mixer
    public AudioMixer audioMixer;

    // Singleton instance property
    public static AudioManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Create audio sources
        musicPlayer = gameObject.AddComponent<AudioSource>();
        soundEffectsPlayer = gameObject.AddComponent<AudioSource>();

        // Set audio mixer
        musicPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        soundEffectsPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SoundEffects")[0];

        // Load player preferences
        LoadPlayerPreferences();
    }

    // Load player preferences
    private void LoadPlayerPreferences()
    {
        GameSettings gameSettings = GameManager.Instance.gameSettings;

        // Set music toggle
        ToggleMusicMute(!gameSettings.isMusicOn);

        // Set sound effects toggle
        ToggleSoundEffectsMute(!gameSettings.isSoundOn);

        // Set music volume
        SetMusicVolume(DEFAULT_MUSIC_VOLUME);

        // Set sound effects volume
        SetSoundEffectsVolume(DEFAULT_SOUND_EFFECTS_VOLUME);
    }

    // Play music
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicPlayer.isPlaying)
        {
            StopMusic();
        }

        musicPlayer.clip = musicClip;
        musicPlayer.volume = GetMusicVolume();
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    // Stop music
    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    // Play sound effect
    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        if (soundEffectClip == null)
        {
            return;
        }

        if (soundEffectsPlayer.isPlaying)
        {
            soundEffectsPlayer.Stop();
        }

        soundEffectsPlayer.PlayOneShot(soundEffectClip, GetSoundEffectsVolume());
    }

    // Set music volume
    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        musicPlayer.volume = volume;

        // Adjust music volume in the audio mixer
        audioMixer.SetFloat(MUSIC_VOLUME_KEY, ConvertToDecibel(volume));
    }

    // Get music volume
    public float GetMusicVolume()
    {
        float volume;
        audioMixer.GetFloat(MUSIC_VOLUME_KEY, out volume);
        return Mathf.Pow(10f, volume / 20f);
    }

    // Mute/unmute music
    public void ToggleMusicMute(bool isMuted)
    {
        musicPlayer.mute = isMuted;

        // Save music toggle
        ClientPrefs.SetMusicToggle(!isMuted);
    }

    // Check if music is muted
    public bool IsMusicMuted()
    {
        return musicPlayer.mute;
    }

    // Set sound effects volume
    public void SetSoundEffectsVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        // Adjust sound effects volume in the audio mixer
        audioMixer.SetFloat(SOUND_EFFECTS_VOLUME_KEY, ConvertToDecibel(volume));
    }

    // Get sound effects volume
    public float GetSoundEffectsVolume()
    {
        float volume;
        audioMixer.GetFloat(SOUND_EFFECTS_VOLUME_KEY, out volume);
        return Mathf.Pow(10f, volume / 20f);
    }

    // Mute/unmute sound effects
    public void ToggleSoundEffectsMute(bool isMuted)
    {
        soundEffectsPlayer.mute = isMuted;

        // Save sound effects toggle
        ClientPrefs.SetSoundEffectsToggle(!isMuted);
    }

    // Check if sound effects are muted
    public bool AreSoundEffectsMuted()
    {
        return soundEffectsPlayer.mute;
    }

    // Convert volume from linear scale to decibel scale
    private float ConvertToDecibel(float volume)
    {
        return Mathf.Log10(volume) * 20f;
    }
}