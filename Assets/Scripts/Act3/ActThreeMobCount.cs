using UnityEngine;

[CreateAssetMenu(fileName = "ActThreeMobCount", menuName = "Scriptable Objects/ActThreeMobCount")]
public class ActThreeMobCount : ScriptableObject
{
    public int mobCount;

    //Reset mob count at start of Act 3 phase one and phase two
    public void ResetMobCount()
    {
        mobCount = 30;
    }

    //Invoke function to decrease mob count whenever a robot mob is destroyed
    public void DecreaseMobCount()
    {
        mobCount -= 1;
    }
}
