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
    private bool isOpen;
    private float previousTimeScale;

    private void Start()
    {
        settingsPanel.SetActive(false);
        masterSlider.SetValueWithoutNotify(audioManager.MasterVolume);
        musicSlider.SetValueWithoutNotify(audioManager.MusicVolume);
        effectsSlider.SetValueWithoutNotify(audioManager.EffectsVolume);
        masterSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
        musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(audioManager.SetEffectsVolume);
        settingsButton.onClick.AddListener(OpenSettings);
        closeButton.onClick.AddListener(CloseSettings);
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
        if (masterSlider != null) masterSlider.onValueChanged.RemoveListener(audioManager.SetMasterVolume);
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(audioManager.SetMusicVolume);
        if (effectsSlider != null) effectsSlider.onValueChanged.RemoveListener(audioManager.SetEffectsVolume);
        if (settingsButton != null) settingsButton.onClick.RemoveListener(OpenSettings);
        if (closeButton != null) closeButton.onClick.RemoveListener(CloseSettings);
    }
}
