using UnityEngine;

// Act1Manager (this is to manage missions, enemy spawns etc)
public class Act1Manager : MonoBehaviour
{
    private enum State { Room1Travel, Room2Secure, Room3Approach, Room3Miniboss, Room3Talk, Done }
    private State state;

    [Header("References")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MissionUI missionUI;

    [Header("Opening Dialogue")]
    [SerializeField] private DialogueData openingDialogue; // leave empty to skip straight to Room 1

    [Header("Mission Text - edit wording here, not in code")]
    [SerializeField] private string room1Header = "Act 1 - Push Deeper Into the City";
    [SerializeField] private string room1Task = "Clear a path forward.";
    [SerializeField] private string room2Header = "Act 1 - Secure the Area";
    [SerializeField] private string room2ClearedTask = "Area secured. Move on.";
    [SerializeField] private string room3Header = "Act 1 - The Miniboss";
    [SerializeField] private string room3MinibossTask = "Defeat the miniboss.";
    [SerializeField] private string room3TalkTask = "Talk to the Swordsman.";
    [SerializeField] private string doneHeader = "Act 1 - Area Secured";
    [SerializeField] private string doneTask = "The Swordsman has joined your party.";

    [Header("Room 1 - Small Group")]
    [SerializeField] private MobManager.EnemyTypes room1EnemyType = MobManager.EnemyTypes.BLUEMOB;
    [SerializeField] private int room1TargetPopulation = 8; // keeps ~7-8 alive at all times, tops up as they die
    private const string Room1WaveName = "room1_wave";

    [Header("Room 2 - Secure Area")]
    [SerializeField] private int secureLevelTarget = 10;
    [SerializeField] private int room2KillDisplayTarget = 15; // just for the task readout, doesn't gate progress
    [SerializeField] private DialogueData room2ClearedDialogue; // plays once level target is hit, before opening the Room 3 gate
    private int killsAtRoom2Start;

    [System.Serializable]
    public struct SpawnWave
    {
        public string coroutineName;
        public MobManager.EnemyTypes type;
        public float interval;
    }
    [SerializeField] private SpawnWave[] room2Waves;

    [Header("Objective Targets (for the direction arrow)")]
    [SerializeField] private Transform room2EntranceTarget;
    [SerializeField] private Transform room3EntranceTarget;
    [SerializeField] private Transform swordsmanTarget;

    [Header("Gates (physically block the way until each stage opens up)")]
    [SerializeField] private GameObject room1To2Gate;
    [SerializeField] private GameObject room2To3Gate;

    [Header("Swordsman")]
    [SerializeField] private Companion swordsmanCompanion; // starts disabled until recruited

    private void Start()
    {
        state = State.Room1Travel;

        if (openingDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(openingDialogue);
            DialogueManager.Instance.onDialogueEnd.AddListener(BeginRoom1);
        }
        else
        {
            BeginRoom1();
        }
    }

    private void BeginRoom1()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(BeginRoom1);
        missionUI?.SetMission(room1Header, room1Task, room2EntranceTarget);
        MobManager.Instance.AddPopulationSpawnCoroutine(Room1WaveName, room1EnemyType, room1TargetPopulation);
    }

    private void Update()
    {
        if (state == State.Room2Secure)
        {
            missionUI?.SetTasks(BuildRoom2Tasks());

            if (stats.level >= secureLevelTarget)
            {
                MobManager.Instance.StopAllSpawnCoroutines();
                state = State.Room3Approach; // set immediately so this block can't re-trigger next frame

                if (room2ClearedDialogue != null)
                {
                    DialogueManager.Instance.StartDialogue(room2ClearedDialogue);
                    DialogueManager.Instance.onDialogueEnd.AddListener(OpenRoom3);
                }
                else
                {
                    OpenRoom3();
                }
            }
        }
    }

    private void OpenRoom3()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(OpenRoom3);
        missionUI?.SetMission(room2Header, room2ClearedTask, room3EntranceTarget);
        if (room2To3Gate != null) room2To3Gate.SetActive(false);
    }

    private string BuildRoom2Tasks()
    {
        int kills = MobManager.Instance.GetKillCount() - killsAtRoom2Start;
        return $"Defeat enemies {kills}/{room2KillDisplayTarget}\nReach level {stats.level}/{secureLevelTarget}";
    }

    // Room1 -> Room2 trigger
    public void EnterRoom2()
    {
        if (state != State.Room1Travel) return;
        MobManager.Instance.StopSpawnCoroutine(Room1WaveName);
        state = State.Room2Secure;
        killsAtRoom2Start = MobManager.Instance.GetKillCount();
        missionUI?.SetMission(room2Header, BuildRoom2Tasks(), null);
        if (room1To2Gate != null) room1To2Gate.SetActive(false);

        foreach (var wave in room2Waves)
            MobManager.Instance.AddSpawnCoroutine(wave.coroutineName, wave.interval, wave.type);
    }

    // Room2 -> Room3 trigger
    public void EnterRoom3()
    {
        if (state != State.Room3Approach) return;
        state = State.Room3Miniboss;
        missionUI?.SetMission(room3Header, room3MinibossTask, null);
    }

    // Called by Act1Miniboss when it dies
    public void OnMinibossDefeated()
    {
        if (state != State.Room3Miniboss) return;
        state = State.Room3Talk;
        missionUI?.SetMission(room3Header, room3TalkTask, swordsmanTarget);
    }

    // Trigger placed near the Swordsman
    public void TalkToSwordsman()
    {
        if (state != State.Room3Talk) return;
        state = State.Done;
        missionUI?.SetMission(doneHeader, doneTask, null);

        if (swordsmanCompanion != null) swordsmanCompanion.enabled = true;
    }
}
