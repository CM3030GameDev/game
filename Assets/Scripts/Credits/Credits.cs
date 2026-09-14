using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Scrolls the credits up from below the screen, then goes back to the start menu
[RequireComponent(typeof(TMP_Text))]
public class Credits : MonoBehaviour
{
    [SerializeField] private TextAsset creditsFile;          // replaces the text in the scene when set
    [SerializeField] private float scrollSpeed = 80f;
    [SerializeField] private float fastMultiplier = 5f;      // while Space or left mouse is held
    [SerializeField] private float endHold = 4f;             // seconds the last lines stay on screen
    [SerializeField] private string menuScene = "StartMenu";
    [SerializeField] private MenuSceneTransition transition; // optional fade, loads directly without it

    private RectTransform rect;
    private float endY;
    private float holdTimer;
    private bool leaving;

    private void Awake() => rect = GetComponent<RectTransform>();

    private void Start()
    {
        // The final boss pauses the game right before loading this scene
        Time.timeScale = 1f;

        TMP_Text text = GetComponent<TMP_Text>();
        if (creditsFile != null) text.text = creditsFile.text;

        // The scene's text had uneven margins (left -236, right -488), which pushed the centre off to one side
        text.margin = Vector4.zero;

        // Anchored to the top at 80 percent width, so the text height decides where the scroll ends
        rect.anchorMin = new Vector2(0.1f, 1f);
        rect.anchorMax = new Vector2(0.9f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.sizeDelta = Vector2.zero;
        float height = text.GetPreferredValues(text.text, rect.rect.width, 0f).y;
        rect.sizeDelta = new Vector2(0f, height);

        // Starts just below the screen and stops once the last lines reach the middle
        float screenHeight = ((RectTransform)rect.parent).rect.height;
        rect.anchoredPosition = new Vector2(0f, -screenHeight);
        endY = height - screenHeight * 0.5f;
    }

    private void Update()
    {
        if (leaving) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMenu();
            return;
        }

        if (rect.anchoredPosition.y < endY)
        {
            bool fast = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);
            rect.anchoredPosition += Vector2.up * (scrollSpeed * (fast ? fastMultiplier : 1f) * Time.unscaledDeltaTime);
            return;
        }

        holdTimer += Time.unscaledDeltaTime;
        if (holdTimer >= endHold) BackToMenu();
    }

    // Public so a Back button OnClick can call it too
    public void BackToMenu()
    {
        if (leaving) return;
        leaving = true;

        if (transition != null) transition.LoadSceneWithFade(menuScene);
        else SceneManager.LoadScene(menuScene);
    }
}
