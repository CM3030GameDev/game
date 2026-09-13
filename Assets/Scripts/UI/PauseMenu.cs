using UnityEngine;
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
    [Tooltip("Control_Panel copied from StartMenu. The Controls button does nothing while empty.")]
    [SerializeField] private GameObject controlsPanel;
    [Tooltip("Audio_Panel copied from StartMenu. The Settings button does nothing while empty.")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private int sortOrder = 50; // above the HUD (0) and below the scene fade (100)

    private float timeScaleBeforePause = 1f;

    public bool IsOpen => gameObject.activeSelf;

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

    // Public so the Back buttons on the copied panels can call it from OnClick
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

    void ControlOption()
    {
        if (controlsPanel != null) ShowOnly(controlsPanel);
    }

    void SettingsOption()
    {
        if (settingsPanel != null) ShowOnly(settingsPanel);
    }

    void ExitOption()
    {
        ShowOnly(confirmation);
    }

    void YesOption()
    {
        Time.timeScale = 1f;
        sceneState.pause = false;
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    void NoOption()
    {
        ShowOptions();
    }
}
