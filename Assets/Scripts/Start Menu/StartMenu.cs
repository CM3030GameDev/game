using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("Main Actions")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartOption);
        }

        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(CreditsOption);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitOption);
        }
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartOption);
        }

        if (creditsButton != null)
        {
            creditsButton.onClick.RemoveListener(CreditsOption);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(ExitOption);
        }
    }

    private void StartOption()
    {
        SceneManager.LoadScene("Act1", LoadSceneMode.Single);
    }

    private void CreditsOption()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    private void ExitOption()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}