using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Events")]
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    private Queue<string> sentences;
    [SerializeField]
    private float nextLetterDelay = 0.02f;
    [SerializeField]
    private float nextSentenceDelay = 1f;
    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
        return;
    }
    

    sentences = new Queue<string>();
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
    }

    public void StartDialogue(DialogueData dialogue)
    {
        if (dialogue == null || dialogue.sentences == null || dialogue.sentences.Length == 0)
        {
            Debug.Log("StartDialogue called with empty or null dialogue.");
            return;
        }

        IsDialogueActive = true;
        dialoguePanel.SetActive(true);

        nameText.text = dialogue.speakerName;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        onDialogueStart?.Invoke();
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(nextLetterDelay);
        }
        yield return new WaitForSeconds(nextSentenceDelay);
        DisplayNextSentence();
    }
    private void EndDialogue()
    {
        IsDialogueActive = false;
        dialoguePanel.SetActive(false);
        onDialogueEnd?.Invoke();
    }
}