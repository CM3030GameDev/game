using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float hoverVolume = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float clickVolume = 0.8f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayHover()
    {
        if (uiAudioSource != null && hoverSound != null)
        {
            uiAudioSource.PlayOneShot(hoverSound, hoverVolume);
        }
    }

    public void PlayClick()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(clickSound, clickVolume);
        }
    }
}