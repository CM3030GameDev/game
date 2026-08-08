using System.Collections;
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

    [Header("UI Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float hoverVolume = 0.6f;

    [Range(0f, 1f)]
    [SerializeField] private float clickVolume = 0.8f;

    [Header("Music Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float targetMusicVolume = 0.3f;

    [SerializeField] private float musicFadeInDuration = 2.5f;
    [SerializeField] private float musicFadeOutDuration = 1f;

    private Coroutine musicFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (musicAudioSource == null)
        {
            Debug.LogWarning("Music Audio Source has not been assigned.");
            return;
        }

        musicAudioSource.volume = 0f;

        if (!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }

        FadeMusicIn();
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

    public void FadeMusicIn()
    {
        StartMusicFade(targetMusicVolume, musicFadeInDuration);
    }

    public void FadeMusicOut()
    {
        StartMusicFade(0f, musicFadeOutDuration);
    }

    public IEnumerator FadeMusicOutAndWait()
    {
        if (musicAudioSource == null)
        {
            yield break;
        }

        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }

        yield return FadeMusicRoutine(0f, musicFadeOutDuration);
    }

    private void StartMusicFade(float targetVolume, float duration)
    {
        if (musicAudioSource == null)
        {
            return;
        }

        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }

        musicFadeCoroutine = StartCoroutine(
            FadeMusicRoutine(targetVolume, duration)
        );
    }

    private IEnumerator FadeMusicRoutine(float targetVolume, float duration)
    {
        float startVolume = musicAudioSource.volume;
        float elapsed = 0f;

        if (duration <= 0f)
        {
            musicAudioSource.volume = targetVolume;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            musicAudioSource.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                elapsed / duration
            );

            yield return null;
        }

        musicAudioSource.volume = targetVolume;
        musicFadeCoroutine = null;
    }
}