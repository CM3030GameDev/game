using UnityEngine;

public class TutorialDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData dialogue;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If this trigger has already activated, do nothing.
        if (triggerOnlyOnce && hasTriggered)
        {
            return;
        }

        // Only the main playable character should activate
        // tutorial dialogue.
        if (!other.CompareTag("Character"))
        {
            return;
        }

        // Make sure the DialogueManager exists.
        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "DialogueManager.Instance could not be found."
            );
            return;
        }

        // Make sure DialogueData has been assigned.
        if (dialogue == null)
        {
            Debug.LogError(
                "No DialogueData assigned to " + gameObject.name
            );
            return;
        }

        Debug.Log(
            "Tutorial Trigger activated: " + dialogue.name
        );

        // Start the assigned dialogue.
        DialogueManager.Instance.StartDialogue(dialogue);

        // Prevent this trigger from activating again.
        hasTriggered = true;
    }
}