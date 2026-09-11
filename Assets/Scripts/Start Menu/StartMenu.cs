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
        if (creditsButton != null)
        {
            creditsButton.onClick.RemoveListener(CreditsOption);
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveListener(ExitOption);
        }
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