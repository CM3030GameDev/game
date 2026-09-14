using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button controlButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject confirmation;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private int sortOrder = 50; // above the HUD (0) and below the scene fade (100)

    [Header("Controls and Settings panels (built at runtime in the Exit button's style)")]
    [Tooltip("One control per line, written as Action | Key.")]
    [TextArea(4, 10)][SerializeField] private string controls =
        "Move | W A S D\n" +
        "Toggle Auto / Manual Aim | Left Click\n" +
        "Aim (Manual) | Mouse\n" +
        "Air Strike | Right Click\n" +
        "Smokescreen | 1\n" +
        "Guard | 2\n" +
        "Pause | Esc";
    [Range(0.05f, 0.5f)][SerializeField] private float volumeStep = 0.1f;
    [Tooltip("Size of the Controls and Settings panels, centred inside the frame (the frame is 1200 x 1100).")]
    [SerializeField] private Vector2 panelSize = new Vector2(900f, 850f);

    private float timeScaleBeforePause = 1f;
    private GameObject controlsPanel;
    private GameObject settingsPanel;
    private TMP_Text musicValue;
    private TMP_Text sfxValue;
    private float baseSize;

    public bool IsOpen => gameObject.activeSelf;

    // Runs the first time the menu opens, so the panels copy the Exit button as it looks in this scene
    private void Awake()
    {
        TMP_Text style = exitButton.GetComponentInChildren<TMP_Text>();
        baseSize = style.fontSize;
        controlsPanel = BuildControlsPanel(style);
        settingsPanel = BuildSettingsPanel(style);
    }

    void Start()
    {
        resumeButton.onClick.AddListener(ResumeOption);
        controlButton.onClick.AddListener(ControlOption);
        settingsButton.onClick.AddListener(SettingsOption);
        exitButton.onClick.AddListener(ExitOption);
        yesButton.onClick.AddListener(YesOption);
        noButton.onClick.AddListener(NoOption);
    }

    public void Open()
    {
        // Remembered so resuming keeps the level up panel or a scene fade paused
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
        sceneState.pause = true;

        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null) canvas.sortingOrder = sortOrder;

        ShowOnly(options);
        gameObject.SetActive(true);
    }

    // Esc steps back one screen, and closes the menu from the main options
    public void Back()
    {
        if (options.activeSelf) ResumeOption();
        else ShowOptions();
    }

    public void ShowOptions() => ShowOnly(options);

    private void ShowOnly(GameObject panel)
    {
        options.SetActive(panel == options);
        confirmation.SetActive(panel == confirmation);
        if (controlsPanel != null) controlsPanel.SetActive(panel == controlsPanel);
        if (settingsPanel != null) settingsPanel.SetActive(panel == settingsPanel);
    }

    void ResumeOption()
    {
        Time.timeScale = timeScaleBeforePause;
        sceneState.pause = false;
        gameObject.SetActive(false);
    }

    void ControlOption() => ShowOnly(controlsPanel);

    void SettingsOption()
    {
        RefreshVolumes();
        ShowOnly(settingsPanel);
    }

    void ExitOption() => ShowOnly(confirmation);

    void YesOption()
    {
        Time.timeScale = 1f;
        sceneState.pause = false;
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    void NoOption() => ShowOptions();

    private GameObject BuildControlsPanel(TMP_Text style)
    {
        GameObject panel = NewPanel("Controls");
        AddText(panel.transform, style, "Controls", baseSize * 0.6f);

        // Two columns per line (action left, key right) using TMP's zero line height trick
        string rows = "";
        foreach (string line in controls.Split('\n'))
        {
            string[] parts = line.Split('|');
            if (parts.Length < 2) continue;
            rows += $"<align=left>{parts[0].Trim()}<line-height=0>\n<align=right>{parts[1].Trim()}</line-height>\n";
        }

        TMP_Text list = AddText(panel.transform, style, rows.TrimEnd('\n'), baseSize * 0.3f);
        list.GetComponent<LayoutElement>().preferredWidth = panelSize.x * 0.9f;

        AddButton(panel.transform, "Back", baseSize * 0.5f, ShowOptions);
        return panel;
    }

    private GameObject BuildSettingsPanel(TMP_Text style)
    {
        GameObject panel = NewPanel("Settings");
        AddText(panel.transform, style, "Settings", baseSize * 0.6f);
        musicValue = AddVolumeRow(panel.transform, style, "Music", true);
        sfxValue = AddVolumeRow(panel.transform, style, "SFX", false);
        AddButton(panel.transform, "Back", baseSize * 0.5f, ShowOptions);
        return panel;
    }

    // Label, minus button, percentage, plus button
    private TMP_Text AddVolumeRow(Transform parent, TMP_Text style, string label, bool music)
    {
        GameObject row = new GameObject(label + "_Row", typeof(RectTransform));
        row.transform.SetParent(parent, false);
        Configure(row.AddComponent<HorizontalLayoutGroup>());

        float size = baseSize * 0.4f;
        AddText(row.transform, style, label, size).GetComponent<LayoutElement>().preferredWidth = size * 4f;
        AddButton(row.transform, "-", size, () => StepVolume(music, -volumeStep));
        TMP_Text value = AddText(row.transform, style, "", size);
        value.GetComponent<LayoutElement>().preferredWidth = size * 3f;
        AddButton(row.transform, "+", size, () => StepVolume(music, volumeStep));
        return value;
    }

    // Centred at a fixed size, because Options is only a 100 x 100 box and a layout inside it squashes everything
    private GameObject NewPanel(string name)
    {
        GameObject panel = new GameObject(name, typeof(RectTransform));
        RectTransform rt = (RectTransform)panel.transform;
        rt.SetParent(options.transform.parent, false);
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = panelSize;

        Configure(panel.AddComponent<VerticalLayoutGroup>());
        panel.SetActive(false);
        return panel;
    }

    private void Configure(HorizontalOrVerticalLayoutGroup group)
    {
        group.childAlignment = TextAnchor.MiddleCenter;
        group.childControlWidth = true;
        group.childControlHeight = true;
        group.childForceExpandWidth = false;
        group.childForceExpandHeight = false;
        group.spacing = baseSize * 0.15f;
    }

    private TMP_Text AddText(Transform parent, TMP_Text style, string text, float size)
    {
        GameObject go = new GameObject("Text", typeof(RectTransform), typeof(LayoutElement));
        go.transform.SetParent(parent, false);

        TextMeshProUGUI t = go.AddComponent<TextMeshProUGUI>();
        t.font = style.font;
        t.fontSharedMaterial = style.fontSharedMaterial;
        t.color = style.color;
        t.fontSize = size;
        t.alignment = TextAlignmentOptions.Center;
        t.raycastTarget = false;
        t.text = text;
        return t;
    }

    // Clones the Exit button so new buttons keep its look, then swaps the label and click action
    private void AddButton(Transform parent, string text, float size, UnityAction onClick)
    {
        GameObject go = Instantiate(exitButton.gameObject, parent);
        go.name = text + "_Button";
        go.SetActive(true);

        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);

        // The scene labels are nudged by hand to look centred on their own words, so reset that for new text
        TMP_Text label = go.GetComponentInChildren<TMP_Text>();
        RectTransform lr = label.rectTransform;
        lr.anchorMin = Vector2.zero;
        lr.anchorMax = Vector2.one;
        lr.anchoredPosition = Vector2.zero;
        lr.sizeDelta = Vector2.zero;
        label.alignment = TextAlignmentOptions.Center;
        label.text = text;
        label.fontSize = size;

        LayoutElement le = go.GetComponent<LayoutElement>();
        if (le == null) le = go.AddComponent<LayoutElement>();
        le.preferredWidth = label.GetPreferredValues(text).x + size * 0.5f;
        le.preferredHeight = size * 1.3f;
    }

    private void StepVolume(bool music, float delta)
    {
        float next = Mathf.Round((UIAudioManager.SavedVolume(music) + delta) / volumeStep) * volumeStep;
        UIAudioManager.SetVolume(music, next);
        RefreshVolumes();
    }

    private void RefreshVolumes()
    {
        if (musicValue != null) musicValue.text = Mathf.RoundToInt(UIAudioManager.SavedVolume(true) * 100f) + "%";
        if (sfxValue != null) sfxValue.text = Mathf.RoundToInt(UIAudioManager.SavedVolume(false) * 100f) + "%";
    }
}
