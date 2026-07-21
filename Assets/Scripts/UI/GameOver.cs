using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Pause game scene
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Go back to start menu after pressing space
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartMenu", LoadSceneMode.Single);
            //Resume game scene
            Time.timeScale = 1f;
        }
    }
}
