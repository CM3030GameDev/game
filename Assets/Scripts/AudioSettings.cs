using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Volume Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Volume Percentage Text")]
    [SerializeField] private TMP_Text musicPercentText;
    [SerializeField] private TMP_Text sfxPercentText;

    private const string MUSIC_VOLUME = "MusicVolume";
    private const string SFX_VOLUME = "SFXVolume";

    private void Start()
    {
        // Load saved volume values.
        float savedMusicVolume =
            PlayerPrefs.GetFloat("MusicVolumeValue", 1f);

        float savedSFXVolume =
            PlayerPrefs.GetFloat("SFXVolumeValue", 1f);

        // Set slider positions.
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;

        // Apply the saved volume settings.
        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        // Listen for slider changes.
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // =========================
    // MUSIC VOLUME
    // =========================

    public void SetMusicVolume(float value)
    {
        float volumeDB = ConvertToDecibels(value);

        audioMixer.SetFloat(MUSIC_VOLUME, volumeDB);

        PlayerPrefs.SetFloat("MusicVolumeValue", value);
        PlayerPrefs.Save();

        UpdateMusicPercentage(value);
    }

    // =========================
    // SFX VOLUME
    // =========================

    public void SetSFXVolume(float value)
    {
        float volumeDB = ConvertToDecibels(value);

        audioMixer.SetFloat(SFX_VOLUME, volumeDB);

        PlayerPrefs.SetFloat("SFXVolumeValue", value);
        PlayerPrefs.Save();

        UpdateSFXPercentage(value);
    }

    // =========================
    // PERCENTAGE DISPLAY
    // =========================

    private void UpdateMusicPercentage(float value)
    {
        if (musicPercentText == null)
        {
            return;
        }

        int percentage = Mathf.RoundToInt(value * 100f);

        musicPercentText.text = percentage + "%";
    }

    private void UpdateSFXPercentage(float value)
    {
        if (sfxPercentText == null)
        {
            return;
        }

        int percentage = Mathf.RoundToInt(value * 100f);

        sfxPercentText.text = percentage + "%";
    }

    // =========================
    // VOLUME CONVERSION
    // =========================

    private float ConvertToDecibels(float value)
    {
        if (value <= 0.0001f)
        {
            return -80f;
        }

        return Mathf.Log10(value) * 20f;
    }
}