using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExitTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData tutorialCompleteDialogue;

    [Header("Transition")]
    [SerializeField] private MenuSceneTransition sceneTransition;
    [SerializeField] private string nextSceneName = "Act1";
    [Tooltip("Card shown on the black screen while the next act loads.")]
    [SerializeField] private string nextActTitle = "Act 1 - City Outskirts";

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

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

        LeaveTutorial();
    }

    // The fade, the music fade and the title card all live in MenuSceneTransition, so every act
    // hands off the same way instead of each one owning a copy of the sequence.
    private void LeaveTutorial()
    {
        if (sceneTransition != null) sceneTransition.LoadSceneWithFade(nextSceneName, nextActTitle);
        else SceneManager.LoadScene(nextSceneName);
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