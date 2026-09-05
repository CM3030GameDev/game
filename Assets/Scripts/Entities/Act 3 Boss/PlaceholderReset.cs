using UnityEngine;

public class PlaceholderReset : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterStats.ResetStats();
        sceneState.ResetStates();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            characterStats.ResetStats();
            sceneState.ResetStates();
        }
    }
}
