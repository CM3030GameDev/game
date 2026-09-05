using UnityEngine;

public class TutorialMercenarySkillTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueData mercenarySkillDialogue;

    [Header("Enemy Spawner")]
    [SerializeField] private TutorialMultiEnemySpawner enemySpawner;

    [Header("Settings")]
    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnlyOnce && hasTriggered)
            return;

        // Your player uses the Character tag
        if (!other.CompareTag("Character"))
            return;

        if (DialogueManager.Instance == null)
        {
            Debug.LogError(
                "TutorialMercenarySkillTrigger: DialogueManager not found."
            );
            return;
        }

        if (mercenarySkillDialogue == null)
        {
            Debug.LogError(
                "TutorialMercenarySkillTrigger: Dialogue not assigned."
            );
            return;
        }

        if (enemySpawner == null)
        {
            Debug.LogError(
                "TutorialMercenarySkillTrigger: Enemy spawner not assigned."
            );
            return;
        }

        hasTriggered = true;
        waitingForDialogue = true;

        DialogueManager.Instance.onDialogueEnd.AddListener(
            OnDialogueFinished
        );

        DialogueManager.Instance.StartDialogue(
            mercenarySkillDialogue
        );

        Debug.Log("Mercenary skill tutorial triggered.");
    }

    private void OnDialogueFinished()
    {
        if (!waitingForDialogue)
            return;

        waitingForDialogue = false;

        DialogueManager.Instance.onDialogueEnd.RemoveListener(
            OnDialogueFinished
        );

        enemySpawner.SpawnEnemies();

        Debug.Log(
            "Mercenary tutorial enemies spawned. Press 1 to use the skill."
        );
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