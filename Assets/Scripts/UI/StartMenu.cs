using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject sound;
    [SerializeField] private GameObject title;
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button backButton1;
    [SerializeField] private Button backButton2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(StartOption);
        settingsButton.onClick.AddListener(SettingsOption);
        creditsButton.onClick.AddListener(CreditsOption);
        exitButton.onClick.AddListener(ExitOption);
        audioButton.onClick.AddListener(AudioOption);
        backButton1.onClick.AddListener(BackOption1);
        backButton2.onClick.AddListener(BackOption2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartOption()
    {
        //Load Act1 Scene
        SceneManager.LoadScene("Act1", LoadSceneMode.Single);
    }

    void SettingsOption()
    {
        title.SetActive(false);
        options.SetActive(false);
        settings.SetActive(true);
    }

    void CreditsOption()
    {
        //Load Credits Scene
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    void ExitOption()
    {
        //Exit game
        Application.Quit();
    }

    void AudioOption()
    {
        settings.SetActive(false);
        sound.SetActive(true);
    }

    void BackOption1()
    {
        settings.SetActive(false);
        title.SetActive(true);
        options.SetActive(true);
    }
    void BackOption2()
    {
        sound.SetActive(false);
        settings.SetActive(true);
    }
}
