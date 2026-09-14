using UnityEngine;

public class Interface : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private CharacterStats characterStats;

    private PauseMenu menu;

    private void Awake()
    {
        menu = pauseMenu.GetComponent<PauseMenu>();
    }

    private void Update()
    {
        //Display game over interface
        if (sceneState.dead)
        {
            gameOver.SetActive(true);
        }

        // Esc opens the pause menu, or steps back through it when already open
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    // Shared by Esc and the HUD pause button
    public void TogglePause()
    {
        if (sceneState.dead) return;

        if (menu.IsOpen) menu.Back();
        else menu.Open();
    }
}
