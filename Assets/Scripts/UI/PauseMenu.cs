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
    [SerializeField] private SceneState sceneState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resumeButton.onClick.AddListener(ResumeOption);
        controlButton.onClick.AddListener(ControlOption);
        settingsButton.onClick.AddListener(SettingsOption);
        exitButton.onClick.AddListener(ExitOption);
        yesButton.onClick.AddListener(YesOption);
        noButton.onClick.AddListener(NoOption);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        Time.timeScale = 0f;
        sceneState.pause = true;
        gameObject.SetActive(true);
    }

    void ResumeOption()
    {
        //Resume game scene
        Time.timeScale = 1f;
        sceneState.pause = false;
        gameObject.SetActive(false);
    }

    void ControlOption()
    {

    }

    void SettingsOption()
    {

    }

    void ExitOption()
    {
        options.SetActive(false);
        //Confirmation prompt
        confirmation.SetActive(true);
    }

    void YesOption()
    {
        //Resume game scene
        Time.timeScale = 1f;
        //Return to start menu
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
    }

    void NoOption()
    {
        confirmation.SetActive(false);
        //Return to pause menu
        options.SetActive(true);
    }
}
