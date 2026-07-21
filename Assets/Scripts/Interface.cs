using UnityEngine;

public class Interface : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject missionInterface;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private CompanionSystem companionSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Display game over interface
        if(companionSystem.allDead)
        {
            gameOver.SetActive(true);
        }

        //Display pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !sceneState.pause && !sceneState.mission)
        {
            pauseMenu.SetActive(true);
            //Pause game scene
            Time.timeScale = 0f;
            sceneState.pause = true;
        }

        //Display mission interface
        if (Input.GetKeyDown(KeyCode.M) && !sceneState.mission && !sceneState.pause)
        {
            missionInterface.SetActive(true);
            //Pause game scene
            Time.timeScale = 0f;
            sceneState.mission = true;
        }
        //Close mission interface
        else if(Input.GetKeyDown(KeyCode.M) && sceneState.mission && !sceneState.pause)
        {
            missionInterface.SetActive(false);
            //Resume game scene
            Time.timeScale = 1f;
            sceneState.mission = false;
        }
    }
}
