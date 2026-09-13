using UnityEngine;

public class Interface : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private CharacterStats characterStats;

    private void Update()
    {
        //Display game over interface
        if (sceneState.dead)
        {
            gameOver.SetActive(true);
        }

        //Display pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !sceneState.pause)
        {
            pauseMenu.SetActive(true);
            //Pause game scene
            Time.timeScale = 0f;
            sceneState.pause = true;
        }
    }
}
