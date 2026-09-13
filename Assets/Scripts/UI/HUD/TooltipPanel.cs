using TMPro;
using UnityEngine;
using UnityEngine.UI;

// One hover tooltip per HUD. Slots ask it to show their text above themselves.
public class TooltipPanel : MonoBehaviour
{
    [Tooltip("The box that appears. Starts hidden; this component must sit on an always-active parent.")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private TMP_Text label;
    [Tooltip("Space between the slot and the tooltip, in canvas units.")]
    [SerializeField] private float gap = 12f;

    public static TooltipPanel Instance { get; private set; }

    private RectTransform currentAnchor;
    private static readonly Vector3[] Corners = new Vector3[4];

    private void Awake()
    {
        Instance = this;

        // A tooltip under the cursor would steal the hover and flicker, so it never takes raycasts.
        if (panel != null)
        {
            foreach (Graphic g in panel.GetComponentsInChildren<Graphic>(true)) g.raycastTarget = false;
            panel.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public static void ShowFor(string text, RectTransform anchor)
    {
        if (Instance != null) Instance.Show(text, anchor);
    }

    // Only hides if this anchor owns the tooltip, since entering the next slot can fire before leaving this one.
    public static void HideFor(RectTransform anchor)
    {
        if (Instance != null && Instance.currentAnchor == anchor) Instance.Hide();
    }

    private void Show(string text, RectTransform anchor)
    {
        if (panel == null || label == null || string.IsNullOrWhiteSpace(text)) return;

        currentAnchor = anchor;
        label.text = text;
        panel.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(panel);   // size must be final before clamping

        // Sit centred above the slot.
        anchor.GetWorldCorners(Corners);                      // 0 bottom-left, 1 top-left, 2 top-right, 3 bottom-right
        panel.pivot = new Vector2(0.5f, 0f);
        panel.position = (Corners[1] + Corners[2]) * 0.5f + Vector3.up * gap * panel.lossyScale.y;

        // Push back inside the screen; the skill slots sit in the bottom-right corner.
        panel.GetWorldCorners(Corners);
        Vector3 shift = Vector3.zero;
        if (Corners[2].x > Screen.width) shift.x = Screen.width - Corners[2].x;
        if (Corners[0].x < 0f) shift.x = -Corners[0].x;
        if (Corners[1].y > Screen.height) shift.y = Screen.height - Corners[1].y;
        panel.position += shift;
    }

    private void Hide()
    {
        currentAnchor = null;
        if (panel != null) panel.gameObject.SetActive(false);
    }
}
