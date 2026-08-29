using UnityEngine;
using TMPro;

// Shared mission HUD driver - lives on the persistent HUD, not tied to any specific Act.
// Each Act's manager (Act1Manager, Act2Manager, etc.) calls into this to update the mission
// header, task list, and direction arrow, instead of each owning these references itself.
public class MissionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text tasksText;
    [SerializeField] private QuestArrow questArrow;

    public void SetHeader(string header)
    {
        if (headerText != null) headerText.text = header;
    }

    public void SetTasks(string tasks)
    {
        if (tasksText != null) tasksText.text = tasks;
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
}
