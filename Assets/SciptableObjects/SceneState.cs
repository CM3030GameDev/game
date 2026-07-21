using UnityEngine;

[CreateAssetMenu(fileName = "SceneState", menuName = "Scriptable Objects/SceneState")]
public class SceneState : ScriptableObject
{
    //Current pause menu state
    public bool pause = false;
    //Current mission interface state
    public bool mission = false;

    //Reset to default
    public void ResetStates()
    {
        pause = false;
        mission = false;
        Time.timeScale = 1f;
    }
}
