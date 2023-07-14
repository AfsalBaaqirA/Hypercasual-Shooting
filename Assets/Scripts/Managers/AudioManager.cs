using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";
    private const float DEFAULT_VOLUME = 0.5f;

    private AudioSource musicPlayer;
    private AudioSource soundEffectsPlayer;
    private AudioMixer audioMixer;

    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        musicPlayer = gameObject.AddComponent<AudioSource>();
        soundEffectsPlayer = gameObject.AddComponent<AudioSource>();

        musicPlayer.outputAudioMixerGroup = musicMixerGroup;
        soundEffectsPlayer.outputAudioMixerGroup = soundEffectsMixerGroup;

        audioMixer = musicMixerGroup.audioMixer;
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
        SetVolume(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        SetVolume(SOUND_EFFECTS_VOLUME_KEY, DEFAULT_VOLUME);
    }

    private void SetVolume(string volumeKey, float volume)
    {
        volume = Mathf.Clamp01(volume);
        audioMixer.SetFloat(volumeKey, ConvertToDecibel(volume));
    }

    private float GetVolume(string volumeKey)
    {
        float volume;
        audioMixer.GetFloat(volumeKey, out volume);
        return Mathf.Pow(10f, volume / 20f);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        StopMusic();

        musicPlayer.clip = musicClip;
        musicPlayer.volume = GetVolume(MUSIC_VOLUME_KEY);
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

        soundEffectsPlayer.PlayOneShot(soundEffectClip, GetVolume(SOUND_EFFECTS_VOLUME_KEY));
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume(MUSIC_VOLUME_KEY, volume);
        musicPlayer.volume = volume;
    }

    public float GetMusicVolume()
    {
        return GetVolume(MUSIC_VOLUME_KEY);
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
        SetVolume(SOUND_EFFECTS_VOLUME_KEY, volume);
    }

    public float GetSoundEffectsVolume()
    {
        return GetVolume(SOUND_EFFECTS_VOLUME_KEY);
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
