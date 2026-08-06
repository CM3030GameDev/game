using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TitleEntrance : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float startDelay = 0.35f;
    [SerializeField] private float fadeDuration = 0.8f;

    [Header("Movement")]
    [SerializeField] private float verticalOffset = 25f;

    [Header("Interaction")]
    [SerializeField] private bool enableInteractionAfterEntrance = true;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Vector2 finalPosition;
    private Vector2 startPosition;

    private Coroutine entranceCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        finalPosition = rectTransform.anchoredPosition;
        startPosition = finalPosition + Vector2.down * verticalOffset;
    }

    private void OnEnable()
    {
        if (canvasGroup == null || rectTransform == null)
        {
            return;
        }

        if (entranceCoroutine != null)
        {
            StopCoroutine(entranceCoroutine);
        }

        ResetEntrance();
        entranceCoroutine = StartCoroutine(PlayEntrance());
    }

    private void OnDisable()
    {
        if (entranceCoroutine != null)
        {
            StopCoroutine(entranceCoroutine);
            entranceCoroutine = null;
        }
    }

    private void ResetEntrance()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        rectTransform.anchoredPosition = startPosition;
    }

    private IEnumerator PlayEntrance()
    {
        yield return new WaitForSecondsRealtime(startDelay);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(elapsed / fadeDuration);
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            canvasGroup.alpha = smoothProgress;

            rectTransform.anchoredPosition = Vector2.Lerp(
                startPosition,
                finalPosition,
                smoothProgress
            );

            yield return null;
        }

        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = finalPosition;

        canvasGroup.interactable = enableInteractionAfterEntrance;
        canvasGroup.blocksRaycasts = enableInteractionAfterEntrance;

        entranceCoroutine = null;
    }
}