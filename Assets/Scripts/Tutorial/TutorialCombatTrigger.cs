using UnityEngine;

public class TutorialCombatTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData combatDialogue;

    [Header("Combat")]
    [SerializeField] private TutorialCombatSpawner combatSpawner;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Do not activate more than once.
        if (triggerOnlyOnce && hasTriggered)
        {
            return;
        }

        // Only the main playable character activates this trigger.
        if (!other.CompareTag("Character"))
        {
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "TutorialCombatTrigger: DialogueManager could not be found."
            );
            return;
        }

        if (combatDialogue == null)
        {
            Debug.LogError(
                "TutorialCombatTrigger: Combat dialogue is not assigned."
            );
            return;
        }

        if (combatSpawner == null)
        {
            Debug.LogError(
                "TutorialCombatTrigger: Combat spawner is not assigned."
            );
            return;
        }

        hasTriggered = true;
        waitingForDialogue = true;

        // Listen for this dialogue finishing.
        DialogueManager.Instance.onDialogueEnd.AddListener(
            OnCombatDialogueFinished
        );

        // Start the combat tutorial dialogue.
        DialogueManager.Instance.StartDialogue(combatDialogue);
    }

    private void OnCombatDialogueFinished()
    {
        if (!waitingForDialogue)
        {
            return;
        }

        waitingForDialogue = false;

        // Remove listener so later dialogues do not spawn another enemy.
        DialogueManager.Instance.onDialogueEnd.RemoveListener(
            OnCombatDialogueFinished
        );

        // Spawn one tutorial enemy.
        combatSpawner.SpawnTutorialEnemy();

        Debug.Log(
            "Combat dialogue finished. Tutorial enemy spawn requested."
        );
    }

    private void OnDestroy()
    {
        // Clean up listener if the object is destroyed.
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.onDialogueEnd.RemoveListener(
                OnCombatDialogueFinished
            );
        }
    }
}