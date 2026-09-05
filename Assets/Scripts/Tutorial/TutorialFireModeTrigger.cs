using UnityEngine;

public class TutorialFireModeTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData fireModeDialogue;

    [Header("Enemy Spawner")]
    [SerializeField] private TutorialMultiEnemySpawner enemySpawner;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnlyOnce && hasTriggered)
        {
            return;
        }

        if (!other.CompareTag("Character"))
        {
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "TutorialFireModeTrigger: DialogueManager not found."
            );

            return;
        }

        if (fireModeDialogue == null)
        {
            Debug.LogError(
                "TutorialFireModeTrigger: Dialogue not assigned."
            );

            return;
        }

        if (enemySpawner == null)
        {
            Debug.LogError(
                "TutorialFireModeTrigger: Enemy spawner not assigned."
            );

            return;
        }

        hasTriggered = true;
        waitingForDialogue = true;

        DialogueManager.Instance.onDialogueEnd.AddListener(
            OnDialogueFinished
        );

        DialogueManager.Instance.StartDialogue(
            fireModeDialogue
        );
    }

    private void OnDialogueFinished()
    {
        if (!waitingForDialogue)
        {
            return;
        }

        waitingForDialogue = false;

        DialogueManager.Instance.onDialogueEnd.RemoveListener(
            OnDialogueFinished
        );

        enemySpawner.SpawnEnemies();
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