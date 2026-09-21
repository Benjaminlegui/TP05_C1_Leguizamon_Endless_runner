using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Text masterPercentageText;
    [SerializeField] private Text musicPercentageText;
    [SerializeField] private Text effectsPercentageText;
    private bool isOpen;
    private float previousTimeScale;

    private void Start()
    {
        settingsPanel.SetActive(false);
        masterSlider.SetValueWithoutNotify(audioManager.MasterVolume);
        musicSlider.SetValueWithoutNotify(audioManager.MusicVolume);
        effectsSlider.SetValueWithoutNotify(audioManager.EffectsVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
        RefreshPercentages();
        settingsButton.onClick.AddListener(OpenSettings);
        closeButton.onClick.AddListener(CloseSettings);
    }

    private void SetMasterVolume(float value)
    {
        audioManager.SetMasterVolume(value);
        RefreshPercentages();
    }

    private void SetMusicVolume(float value)
    {
        audioManager.SetMusicVolume(value);
        RefreshPercentages();
    }

    private void SetEffectsVolume(float value)
    {
        audioManager.SetEffectsVolume(value);
        RefreshPercentages();
    }

    private void RefreshPercentages()
    {
        masterPercentageText.text = $"{Mathf.RoundToInt(masterSlider.value * 100f)}%";
        musicPercentageText.text = $"{Mathf.RoundToInt(musicSlider.value * 100f)}%";
        effectsPercentageText.text = $"{Mathf.RoundToInt(effectsSlider.value * 100f)}%";
    }

    public void OpenSettings()
    {
        if (isOpen) return;
        isOpen = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (!isOpen) return;
        isOpen = false;
        Time.timeScale = previousTimeScale;
        settingsPanel.SetActive(false);
        PlayerPrefs.Save();
    }

    private void OnDisable() => CloseSettings();

    private void OnDestroy()
    {
        if (masterSlider != null) masterSlider.onValueChanged.RemoveListener(SetMasterVolume);
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (effectsSlider != null) effectsSlider.onValueChanged.RemoveListener(SetEffectsVolume);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(OpenSettings);
        if (closeButton != null) closeButton.onClick.RemoveListener(CloseSettings);
    }
}
