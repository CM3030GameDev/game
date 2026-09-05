using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager for UI sound effects and menu background music.
///
/// This script handles:
/// - Button hover sounds
/// - Button click sounds
/// - Back / confirm / error sounds
/// - Panel open / close sounds
/// - Background music fade in and fade out
///
/// Other scripts can access this manager through:
/// UIAudioManager.Instance
/// </summary>
public class UIAudioManager : MonoBehaviour
{
    // Singleton instance.
    // Allows other scripts to access the Audio Manager without
    // needing a direct reference in the Inspector.
    public static UIAudioManager Instance { get; private set; }


    // ============================================================
    // AUDIO SOURCES
    // ============================================================

    [Header("Audio Sources")]

    // AudioSource used for short UI sound effects.
    [SerializeField] private AudioSource uiAudioSource;

    // Separate AudioSource used for background music.
    // Keeping music separate allows us to fade the music without
    // changing the volume of UI sound effects.
    [SerializeField] private AudioSource musicAudioSource;


    // ============================================================
    // UI SOUND EFFECTS
    // ============================================================

    [Header("UI Sounds")]

    // Played when the mouse moves over a UI button.
    [SerializeField] private AudioClip hoverSound;

    // Played when a normal UI button is clicked.
    [SerializeField] private AudioClip clickSound;

    // Played when the player presses a Back button.
    [SerializeField] private AudioClip backSound;

    // Played for important confirmation actions,
    // such as starting the game.
    [SerializeField] private AudioClip confirmSound;

    // Played when an action cannot be performed.
    [SerializeField] private AudioClip errorSound;

    // Played when a UI panel/menu is opened.
    [SerializeField] private AudioClip panelOpenSound;

    // Played when a UI panel/menu is closed.
    [SerializeField] private AudioClip panelCloseSound;

    // ============================================================
    // INGAME BACKGROUND MUSIC
    // ============================================================

    [Header("Background Music")]

    [SerializeField] private List<AudioClip> backgroundMusic;

    // ============================================================
    // INGAME SOUND EFFECTS
    // ============================================================

    [Header("Sound Effects")]

    [SerializeField] private List<AudioClip> soundEffects;

    // ============================================================
    // UI VOLUME SETTINGS
    // ============================================================

    [Header("UI Volume")]

    // Background music volume
    [Range(0f, 1f)]
    [SerializeField] private float bgmVolume = 0.6f;

    // Sound Effect Volume
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 0.8f;

    // ============================================================
    // MUSIC SETTINGS
    // ============================================================

    [Header("Music Settings")]

    // Final volume that the background music fades towards.
    [Range(0f, 1f)]
    [SerializeField] private float targetMusicVolume = 0.3f;

    // Time taken for music to fade in when the menu starts.
    [SerializeField] private float musicFadeInDuration = 2.5f;

    // Time taken for music to fade out.
    [SerializeField] private float musicFadeOutDuration = 1f;

    // Stores the currently running music fade.
    // This allows us to stop an existing fade before starting another.
    private Coroutine musicFadeCoroutine;

    // ============================================================
    // UNITY LIFECYCLE
    // ============================================================

    private void Awake()
    {
        // Singleton check:
        // If another UIAudioManager already exists, destroy this
        // duplicate so that only one manager controls the audio.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Store this object as the active UIAudioManager.
        Instance = this;
    }


    private void Start()
    {
        // Prevent errors if the Music Audio Source was not assigned
        // in the Unity Inspector.
        if (musicAudioSource == null)
        {
            Debug.LogWarning(
                "Music Audio Source has not been assigned."
            );

            return;
        }

        // Start the music silently so that it can fade in smoothly.
        musicAudioSource.volume = 0f;

        // Start playing the music if it is not already playing.
        if (!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }

        // Gradually increase the music to the target volume.
        FadeMusicIn();
    }

    // ============================================================
    // INGAME BACKGROUND MUSIC & SOUNDEFFECT METHODS
    // ============================================================

    public void PlayBGM(int index)
    {
        musicAudioSource.PlayOneShot(backgroundMusic[index], sfxVolume);
    }

    public void PlaySFX(int index)
    {
        uiAudioSource.PlayOneShot(soundEffects[index], sfxVolume);
    }

    // ============================================================
    // UI SOUND METHODS
    // ============================================================

    /// <summary>
    /// Plays when the mouse pointer enters a UI button.
    /// </summary>
    public void PlayHover()
    {
        if (uiAudioSource != null && hoverSound != null)
        {
            uiAudioSource.PlayOneShot(
                hoverSound,
                sfxVolume
            );
        }
    }


    /// <summary>
    /// Plays the standard button click sound.
    /// </summary>
    public void PlayClick()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(
                clickSound,
                sfxVolume
            );
        }
    }


    /// <summary>
    /// Plays when the player uses a Back button.
    /// </summary>
    public void PlayBack()
    {
        if (uiAudioSource != null && backSound != null)
        {
            uiAudioSource.PlayOneShot(
                backSound,
                sfxVolume
            );
        }
    }


    /// <summary>
    /// Plays for important confirmation actions,
    /// such as pressing Start Game.
    /// </summary>
    public void PlayConfirm()
    {
        if (uiAudioSource != null && confirmSound != null)
        {
            uiAudioSource.PlayOneShot(
                confirmSound,
                sfxVolume
            );
        }
    }


    /// <summary>
    /// Plays when the player attempts an invalid
    /// or unavailable action.
    /// </summary>
    public void PlayError()
    {
        if (uiAudioSource != null && errorSound != null)
        {
            uiAudioSource.PlayOneShot(
                errorSound,
                sfxVolume
            );
        }
    }


    /// <summary>
    /// Plays when a menu or UI panel is opened.
    /// </summary>
    public void PlayPanelOpen()
    {
        if (uiAudioSource != null && panelOpenSound != null)
        {
            uiAudioSource.PlayOneShot(
                panelOpenSound,
                sfxVolume
            );
        }
    }


    /// <summary>
    /// Plays when a menu or UI panel is closed.
    /// </summary>
    public void PlayPanelClose()
    {
        if (uiAudioSource != null && panelCloseSound != null)
        {
            uiAudioSource.PlayOneShot(
                panelCloseSound,
                sfxVolume
            );
        }
    }


    // ============================================================
    // MUSIC CONTROL
    // ============================================================

    /// <summary>
    /// Gradually increases the music volume to the target volume.
    /// </summary>
    public void FadeMusicIn()
    {
        StartMusicFade(
            targetMusicVolume,
            musicFadeInDuration
        );
    }


    /// <summary>
    /// Gradually reduces the music volume to zero.
    /// </summary>
    public void FadeMusicOut()
    {
        StartMusicFade(
            0f,
            musicFadeOutDuration
        );
    }


    /// <summary>
    /// Fades the music out and waits until the fade has finished.
    ///
    /// This is useful before changing scenes because another script
    /// can wait for the music to finish fading before loading the
    /// next scene.
    /// </summary>
    public IEnumerator FadeMusicOutAndWait()
    {
        // If no music source exists, there is nothing to fade.
        if (musicAudioSource == null)
        {
            yield break;
        }

        // Stop any fade that is already running.
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }

        // Wait until the fade-out has completely finished.
        yield return FadeMusicRoutine(
            0f,
            musicFadeOutDuration
        );
    }


    /// <summary>
    /// Starts a new music fade.
    /// Stops the previous fade first to prevent two fades
    /// from changing the volume at the same time.
    /// </summary>
    private void StartMusicFade(
        float targetVolume,
        float duration
    )
    {
        if (musicAudioSource == null)
        {
            return;
        }

        // Stop the previous fade if one is running.
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }

        // Start the new fade.
        musicFadeCoroutine = StartCoroutine(
            FadeMusicRoutine(
                targetVolume,
                duration
            )
        );
    }

    /// <summary>
    /// Coroutine responsible for smoothly changing the
    /// background music volume over time.
    /// </summary>
    private IEnumerator FadeMusicRoutine(
        float targetVolume,
        float duration
    )
    {
        // Remember the current volume before beginning the fade.
        float startVolume = musicAudioSource.volume;

        float elapsed = 0f;


        // If the duration is zero or negative,
        // change the volume immediately.
        if (duration <= 0f)
        {
            musicAudioSource.volume = targetVolume;
            yield break;
        }


        // Gradually move from the current volume
        // to the requested target volume.
        while (elapsed < duration)
        {
            // unscaledDeltaTime means the fade still works
            // even if Time.timeScale is 0 (for example, in a pause menu).
            elapsed += Time.unscaledDeltaTime;

            musicAudioSource.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                elapsed / duration
            );

            // Wait until the next frame.
            yield return null;
        }


        // Make sure the final volume is exactly the target value.
        musicAudioSource.volume = targetVolume;

        // The fade has finished, so clear the coroutine reference.
        musicFadeCoroutine = null;
    }
}