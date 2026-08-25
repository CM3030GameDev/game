using UnityEngine;

// Drives Act1's enemy spawning through MobManager, same way Act2Manager does for Act2.
public class Act1Manager : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnWave
    {
        public string coroutineName;   // must be unique across all active waves
        public MobManager.EnemyTypes type;
        public float interval;         // seconds between spawns of this type
    }

    [SerializeField] private SpawnWave[] waves;

    private void Start()
    {
        foreach (var wave in waves)
        {
            MobManager.Instance.AddSpawnCoroutine(wave.coroutineName, wave.interval, wave.type);
        }
    }
}
