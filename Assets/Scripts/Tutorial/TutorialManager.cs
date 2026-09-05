using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Dialogue")]
    [SerializeField] private DialogueData introDialogue;

    private void Start()
    {
        // Start the introduction dialogue when the tutorial scene loads.
        StartIntroDialogue();
    }

    private void StartIntroDialogue()
    {
        if (introDialogue == null)
        {
            Debug.LogWarning("Tutorial Intro Dialogue has not been assigned.");
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("DialogueManager could not be found.");
            return;
        }

        DialogueManager.Instance.StartDialogue(introDialogue);
    }
}