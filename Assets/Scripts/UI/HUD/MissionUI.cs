using UnityEngine;
using TMPro;

// Shared mission HUD driver - lives on the persistent HUD, not tied to any specific Act.
// Each Act's manager (Act1Manager, Act2Manager, etc.) calls into this to update the mission
// header, task list, and direction arrow, instead of each owning these references itself.
// Hides itself whenever there's no mission text, so the panel doesn't sit on screen showing
// placeholder strings before an Act sets its first objective.
public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;   // root to show/hide; falls back to this object
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text tasksText;
    [SerializeField] private QuestArrow questArrow;

    private void Awake()
    {
        if (panel == null) panel = gameObject;

        // Clear whatever placeholder text was authored in the scene, then hide
        if (headerText != null) headerText.text = string.Empty;
        if (tasksText != null) tasksText.text = string.Empty;
        panel.SetActive(false);
    }

    public void SetHeader(string header)
    {
        if (headerText != null) headerText.text = header;
        RefreshVisibility();
    }

    public void SetTasks(string tasks)
    {
        if (tasksText != null) tasksText.text = tasks;
        RefreshVisibility();
    }

    public void SetArrowTarget(Transform target)
    {
        if (questArrow != null) questArrow.SetTarget(target);
    }

    // Convenience for a full state transition - sets all three at once
    public void SetMission(string header, string tasks, Transform arrowTarget)
    {
        SetHeader(header);
        SetTasks(tasks);
        SetArrowTarget(arrowTarget);
    }

    /// <summary>Hide the panel and drop the arrow - for cutscenes, or when an Act finishes.</summary>
    public void Hide()
    {
        if (headerText != null) headerText.text = string.Empty;
        if (tasksText != null) tasksText.text = string.Empty;
        SetArrowTarget(null);
        if (panel != null) panel.SetActive(false);
    }

    private void RefreshVisibility()
    {
        bool hasContent =
            (headerText != null && !string.IsNullOrWhiteSpace(headerText.text)) ||
            (tasksText != null && !string.IsNullOrWhiteSpace(tasksText.text));

        if (panel != null) panel.SetActive(hasContent);
    }
}
