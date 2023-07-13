using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";
    private const float DEFAULT_MUSIC_VOLUME = 0.5f;
    private const float DEFAULT_SOUND_EFFECTS_VOLUME = 0.5f;

    private AudioSource musicPlayer;
    private AudioSource soundEffectsPlayer;
    public AudioMixer audioMixer;

    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        musicPlayer = gameObject.AddComponent<AudioSource>();
        soundEffectsPlayer = gameObject.AddComponent<AudioSource>();

        musicPlayer.outputAudioMixerGroup = musicMixerGroup;
        soundEffectsPlayer.outputAudioMixerGroup = soundEffectsMixerGroup;
    }

    private void Start()
    {
        LoadPlayerPreferences();
        ClientPrefs.ResetClientPrefs();
    }

    private void LoadPlayerPreferences()
    {
        musicPlayer.mute = ClientPrefs.GetMusicToggle();
        soundEffectsPlayer.mute = ClientPrefs.GetSoundEffectsToggle();
        SetMusicVolume(DEFAULT_MUSIC_VOLUME);
        SetSoundEffectsVolume(DEFAULT_SOUND_EFFECTS_VOLUME);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicPlayer.isPlaying)
            StopMusic();

        musicPlayer.clip = musicClip;
        musicPlayer.volume = GetMusicVolume();
        musicPlayer.loop = true;
        musicPlayer.Play();
    }

    public void StopMusic()
    {
        musicPlayer.Stop();
    }

    public void PlaySoundEffect(AudioClip soundEffectClip)
    {
        if (soundEffectClip == null)
            return;

        if (soundEffectsPlayer.isPlaying)
            soundEffectsPlayer.Stop();

        soundEffectsPlayer.PlayOneShot(soundEffectClip, GetSoundEffectsVolume());
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        musicPlayer.volume = volume;
        audioMixer.SetFloat(MUSIC_VOLUME_KEY, ConvertToDecibel(volume));
    }

    public float GetMusicVolume()
    {
        float volume;
        audioMixer.GetFloat(MUSIC_VOLUME_KEY, out volume);
        return Mathf.Pow(10f, volume / 20f);
    }

    public void ToggleMusicMute(bool isMuted)
    {
        musicPlayer.mute = isMuted;
        ClientPrefs.SetMusicToggle(!isMuted);
    }

    public bool IsMusicMuted()
    {
        return musicPlayer.mute;
    }

    public void SetSoundEffectsVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioMixer.SetFloat(SOUND_EFFECTS_VOLUME_KEY, ConvertToDecibel(volume));
    }

    public float GetSoundEffectsVolume()
    {
        float volume;
        audioMixer.GetFloat(SOUND_EFFECTS_VOLUME_KEY, out volume);
        return Mathf.Pow(10f, volume / 20f);
    }

    public void ToggleSoundEffectsMute(bool isMuted)
    {
        soundEffectsPlayer.mute = isMuted;
        ClientPrefs.SetSoundEffectsToggle(!isMuted);
    }

    public bool AreSoundEffectsMuted()
    {
        return soundEffectsPlayer.mute;
    }

    private float ConvertToDecibel(float volume)
    {
        return Mathf.Log10(volume) * 20f;
    }
}
