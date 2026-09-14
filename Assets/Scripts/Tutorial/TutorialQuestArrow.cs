using UnityEngine;

// Points the quest arrow at each tutorial trigger in order, moving on once the player touches one
public class TutorialQuestArrow : MonoBehaviour
{
    [SerializeField] private MissionUI missionUI;
    [Tooltip("Tutorial trigger colliders in the order the player should reach them")]
    [SerializeField] private Collider2D[] steps;

    private Collider2D player;
    private int current;

    // Start (not Awake) so QuestArrow has already hidden itself before the first target is set
    private void Start()
    {
        GameObject go = GameObject.FindWithTag("Character");
        if (go != null) player = go.GetComponent<Collider2D>();
        SetStep(0);
    }

    private void Update()
    {
        if (player == null || current >= steps.Length) return;

        // Checks later steps too, so walking past one out of order never strands the arrow
        for (int i = steps.Length - 1; i >= current; i--)
        {
            if (steps[i] != null && player.IsTouching(steps[i]))
            {
                SetStep(i + 1);
                return;
            }
        }
    }

    private void SetStep(int index)
    {
        current = index;
        if (missionUI != null) missionUI.SetArrowTarget(index < steps.Length && steps[index] != null ? steps[index].transform : null);
    }
}
