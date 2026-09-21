using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region MixerConstants
        private const string MasterVolumePrefsKey = "Runner.MasterVolume";
        private const string MusicVolumePrefsKey = "Runner.MusicVolume";
        private const string EffectsVolumePrefsKey = "Runner.EffectsVolume";

        private const string MasterVolumeMixerParameter = "MasterVolume";
        private const string MusicVolumeMixerParameter = "MusicVolume";
        private const string EffectsVolumeMixerParameter = "EffectsVolume";
    #endregion

    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;
    [SerializeField] private AudioClip loseSound;

    #region SettersGetters
        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float EffectsVolume { get; private set; }
    
    #endregion
    
    private GameManager.GameState previousState;

    private void Awake()
    {
        MasterVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MasterVolumePrefsKey, 1f));
        MusicVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(MusicVolumePrefsKey, 0.7f));
        EffectsVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(EffectsVolumePrefsKey, 1f));
        musicSource.playOnAwake = false;
        effectsSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = effectsSource.spatialBlend = 0f;
    }

    private void OnEnable()
    {
        previousState = gameManager.State;
        gameManager.Changed += OnGameChanged;
        player.Jumped += PlayJump;
        player.Landed += PlayLanding;
    }

    private void Start()
    {
        ApplyVolume(MasterVolumeMixerParameter, MasterVolume);
        ApplyVolume(MusicVolumeMixerParameter, MusicVolume);
        ApplyVolume(EffectsVolumeMixerParameter, EffectsVolume);
        if (gameManager.IsPlaying) PlayMusic();
    }

    private void OnDisable()
    {
        gameManager.Changed -= OnGameChanged;
        player.Jumped -= PlayJump;
        player.Landed -= PlayLanding;
        musicSource.Stop();
        effectsSource.Stop();
    }

    private void OnGameChanged()
    {
        if (previousState == gameManager.State) return;
        previousState = gameManager.State;
        if (gameManager.IsPlaying)
        {
            effectsSource.Stop();
            PlayMusic();
        }
        else
        {
            musicSource.Stop();
            effectsSource.Stop();
            if (gameManager.State == GameManager.GameState.GameOver) PlayEffect(loseSound);
        }
    }

    private void PlayMusic()
    {
        musicSource.Stop();
        musicSource.clip = gameplayMusic;
        if (gameplayMusic != null) musicSource.Play();
    }

    private void PlayJump() => PlayEffect(jumpSound);
    private void PlayLanding() => PlayEffect(landingSound);
    private void PlayEffect(AudioClip clip)
    {
        if (clip != null) effectsSource.PlayOneShot(clip);
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Clamp01(value);
        ApplyVolume(MasterVolumeMixerParameter, MasterVolume);
        PlayerPrefs.SetFloat(MasterVolumePrefsKey, MasterVolume);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        ApplyVolume(MusicVolumeMixerParameter, MusicVolume);
        PlayerPrefs.SetFloat(MusicVolumePrefsKey, MusicVolume);
    }

    public void SetEffectsVolume(float value)
    {
        EffectsVolume = Mathf.Clamp01(value);
        ApplyVolume(EffectsVolumeMixerParameter, EffectsVolume);
        PlayerPrefs.SetFloat(EffectsVolumePrefsKey, EffectsVolume);
    }

    private void ApplyVolume(string parameter, float value)
    {
        float decibels = value <= 0f ? -80f : Mathf.Log10(value) * 20f;
        if (!audioMixer.SetFloat(parameter, decibels))
            Debug.LogError("Missing exposed AudioMixer parameter: " + parameter, this);
    }
}
