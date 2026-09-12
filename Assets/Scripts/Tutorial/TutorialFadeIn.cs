using System.Collections;
using UnityEngine;

public class TutorialFadeIn : MonoBehaviour
{
    [Header("Fade Overlay")]
    [SerializeField] private CanvasGroup fadeOverlay;

    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 0.8f;

    private void Awake()
    {
        if (fadeOverlay == null)
        {
            Debug.LogError(
                "TutorialFadeIn: Fade Overlay is not assigned."
            );
            return;
        }

        // Start completely black.
        fadeOverlay.alpha = 1f;
        fadeOverlay.blocksRaycasts = true;
        fadeOverlay.interactable = false;
    }

    private void Start()
    {
        Debug.Log("TUTORIAL FADE IN STARTED");
        if (fadeOverlay != null)
        {
            StartCoroutine(FadeFromBlack());
        }
    }

    private IEnumerator FadeFromBlack()
    {
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            fadeOverlay.alpha = Mathf.Lerp(
                1f,
                0f,
                elapsed / fadeInDuration
            );

            yield return null;
        }

        fadeOverlay.alpha = 0f;
        fadeOverlay.blocksRaycasts = false;
    }
}