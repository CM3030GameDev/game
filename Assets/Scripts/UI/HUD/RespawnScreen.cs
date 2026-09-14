using TMPro;
using UnityEngine;

// Fades in a death overlay with a countdown while PlayerRespawn waits to bring the player back
[RequireComponent(typeof(CanvasGroup))]
public class RespawnScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [Tooltip("{0} is replaced with the seconds left.")]
    [SerializeField] private string countdownFormat = "Respawning in {0}";
    [SerializeField] private float fadeSpeed = 4f;

    private CanvasGroup group;
    private float remaining;
    private bool showing;

    // The object stays active and only its alpha changes, so it keeps listening while hidden
    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    private void OnEnable()
    {
        PlayerRespawn.Died += Show;
        PlayerRespawn.Respawned += Hide;
    }

    private void OnDisable()
    {
        PlayerRespawn.Died -= Show;
        PlayerRespawn.Respawned -= Hide;
    }

    private void Show(float delay)
    {
        remaining = delay;
        showing = true;
    }

    private void Hide() => showing = false;

    private void Update()
    {
        group.alpha = Mathf.MoveTowards(group.alpha, showing ? 1f : 0f, fadeSpeed * Time.unscaledDeltaTime);
        if (!showing) return;

        // Scaled time, to stay in step with the WaitForSeconds in PlayerRespawn
        remaining = Mathf.Max(0f, remaining - Time.deltaTime);
        if (countdownText != null) countdownText.text = string.Format(countdownFormat, Mathf.CeilToInt(remaining));
    }
}
