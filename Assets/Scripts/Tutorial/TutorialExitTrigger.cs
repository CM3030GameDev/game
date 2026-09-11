using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExitTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData tutorialCompleteDialogue;

    [Header("Fade Overlay")]
    [SerializeField] private CanvasGroup fadeOverlay;

    [Header("Transition")]
    [SerializeField] private string nextSceneName = "Act1";
    [SerializeField] private float fadeToBlackDuration = 1f;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    private void Start()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.alpha = 0f;
            fadeOverlay.blocksRaycasts = false;
            fadeOverlay.interactable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnlyOnce && hasTriggered)
            return;

        if (!other.CompareTag("Character"))
            return;

        hasTriggered = true;

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("TutorialExitTrigger: DialogueManager not found.");
            return;
        }

        if (tutorialCompleteDialogue == null)
        {
            Debug.LogError("TutorialExitTrigger: Tutorial Complete dialogue not assigned.");
            return;
        }

        waitingForDialogue = true;

        DialogueManager.Instance.onDialogueEnd.AddListener(
            OnDialogueFinished
        );

        DialogueManager.Instance.StartDialogue(
            tutorialCompleteDialogue
        );
    }

    private void OnDialogueFinished()
    {
        if (!waitingForDialogue)
            return;

        waitingForDialogue = false;

        DialogueManager.Instance.onDialogueEnd.RemoveListener(
            OnDialogueFinished
        );

        StartCoroutine(TransitionToAct1());
    }

    private IEnumerator TransitionToAct1()
    {
        // Stop the player interacting with UI while transitioning.
        if (fadeOverlay != null)
        {
            fadeOverlay.blocksRaycasts = true;
            fadeOverlay.interactable = true;

            float startAlpha = fadeOverlay.alpha;
            float elapsed = 0f;

            while (elapsed < fadeToBlackDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                fadeOverlay.alpha = Mathf.Lerp(
                    startAlpha,
                    1f,
                    elapsed / fadeToBlackDuration
                );

                yield return null;
            }

            // Ensure completely black before scene change.
            fadeOverlay.alpha = 1f;
        }
        else
        {
            Debug.LogWarning(
                "TutorialExitTrigger: Fade Overlay is not assigned."
            );

            yield return new WaitForSecondsRealtime(
                fadeToBlackDuration
            );
        }

        // Fade music out after / during the screen transition.
        if (UIAudioManager.Instance != null)
        {
            yield return UIAudioManager.Instance.FadeMusicOutAndWait();
        }

        // Small pause while fully black.
        yield return new WaitForSecondsRealtime(0.2f);

        SceneManager.LoadScene(nextSceneName);
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.onDialogueEnd.RemoveListener(
                OnDialogueFinished
            );
        }
    }
}