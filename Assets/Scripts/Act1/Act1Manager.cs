using UnityEngine;
using UnityEngine.SceneManagement;

// Act1Manager (this is to manage missions, enemy spawns etc)
public class Act1Manager : MonoBehaviour
{
    private enum State { Room1Travel, Room2Secure, Room2Miniboss, Room3Transition, Done }
    private State state;

    [Header("References")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MissionUI missionUI;

    [Header("Opening Dialogue")]
    [SerializeField] private DialogueData openingDialogue; // leave this empty if you guys want to skip straight to Room 1

    [Header("Mission Text - edit wording here, not in code")]
    [SerializeField] private string room1Header = "Act 1 - Push Deeper Into the City";
    [SerializeField] private string room1Task = "Clear a path forward.";
    [SerializeField] private string room2Header = "Act 1 - Secure the Area";
    [SerializeField] private string room2MinibossTask = "Defeat the miniboss.";
    [SerializeField] private string room3Header = "Act 1 - Fall Back";
    [SerializeField] private string room3Task = "Move to the extraction point.";

    [Header("Room 1 - Small Group")]
    [SerializeField] private MobManager.EnemyTypes room1EnemyType = MobManager.EnemyTypes.BLUEMOB;
    [SerializeField] private int room1TargetPopulation = 8; // keeps ~7-8 alive at all times, tops up as they die
    private const string Room1WaveName = "room1_wave";

    [Header("Room 2 - Secure Area")]
    // Both objectives must be met before the miniboss spawns
    [SerializeField] private int secureLevelTarget = 10;
    [UnityEngine.Serialization.FormerlySerializedAs("room2KillDisplayTarget")]
    [SerializeField] private int room2KillTarget = 15;
    private int killsAtRoom2Start;

    [System.Serializable]
    public struct SpawnWave
    {
        public string coroutineName;
        public MobManager.EnemyTypes type;
        public float interval;
    }
    [SerializeField] private SpawnWave[] room2Waves;

    [Header("Room 2 - Miniboss")]
    [SerializeField] private GameObject room2MinibossObject; // starts disabled; wire its death event to OnMinibossDefeated()
    [SerializeField] private DialogueData minibossSpawnDialogue; // plays once the area is secured, before the miniboss appears
    [SerializeField] private DialogueData minibossCutscene; // plays once the miniboss dies, before the Swordsman joins

    [Header("Objective Targets (for the direction arrow)")]
    [SerializeField] private Transform room2EntranceTarget;
    [SerializeField] private Transform room3EntranceTarget;

    [Header("Gates (physically block the way until each stage opens up)")]
    [SerializeField] private GameObject room1To2Gate;
    [SerializeField] private GameObject room2To3Gate;

    [Header("Swordsman")]
    [SerializeField] private Companion swordsmanCompanion; // starts disabled, auto-enabled after the cutscene

    [Header("Act Transition")]
    [SerializeField] private string nextSceneName = "Act4";

    private void Start()
    {
        state = State.Room1Travel;
        PlayThen(openingDialogue, BeginRoom1);
    }

    // Plays a dialogue then continues, hiding the mission panel so it does not sit over it.
    private void PlayThen(DialogueData dialogue, UnityEngine.Events.UnityAction next)
    {
        if (dialogue == null)
        {
            next();
            return;
        }

        missionUI?.Hide();
        DialogueManager.Instance.StartDialogue(dialogue);
        DialogueManager.Instance.onDialogueEnd.AddListener(next);
    }

    private void BeginRoom1()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(BeginRoom1);
        missionUI?.SetMission(room1Header, room1Task, room2EntranceTarget);
        MobManager.Instance.AddPopulationSpawnCoroutine(Room1WaveName, room1EnemyType, room1TargetPopulation);
    }

    private int lastShownKills = -1;
    private int lastShownLevel = -1;

    private void Update()
    {
        if (state != State.Room2Secure) return;

        // Only rebuild the HUD text when a number actually changed, since this runs every frame.
        int kills = MobManager.Instance.GetKillCount() - killsAtRoom2Start;
        if (kills != lastShownKills || stats.level != lastShownLevel)
        {
            lastShownKills = kills;
            lastShownLevel = stats.level;
            missionUI?.SetTasks(BuildRoom2Tasks());
        }

        if (stats.level >= secureLevelTarget && kills >= room2KillTarget)
        {
            MobManager.Instance.StopAllSpawnCoroutines();
            state = State.Room2Miniboss; // set immediately so this block can't re-trigger next frame
            PlayThen(minibossSpawnDialogue, SpawnMiniboss);
        }
    }

    private void SpawnMiniboss()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(SpawnMiniboss);

        if (room2MinibossObject != null)
        {
            room2MinibossObject.SetActive(true);
            // Subscribed in code because a UnityEvent dragged onto the wrong object fails silently.
            room2MinibossObject.GetComponent<Act1Boss>()?.bossDeath.AddListener(OnMinibossDefeated);
        }

        // Point the arrow at the miniboss so the player can find it in a big room
        Transform target = room2MinibossObject != null ? room2MinibossObject.transform : null;
        missionUI?.SetMission(room2Header, room2MinibossTask, target);
    }

    private string BuildRoom2Tasks()
    {
        int kills = Mathf.Min(MobManager.Instance.GetKillCount() - killsAtRoom2Start, room2KillTarget);
        int level = Mathf.Min(stats.level, secureLevelTarget);

        return Objective($"Defeat enemies {kills}/{room2KillTarget}", kills >= room2KillTarget) + "\n" +
               Objective($"Reach level {level}/{secureLevelTarget}", level >= secureLevelTarget);
    }

    // Completed objectives turn green, so the player can see which half is still outstanding
    private static string Objective(string text, bool done)
        => done ? $"<color=#7CFC7C>{text}</color>" : text;

    // Room 1 to Room 2 trigger
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

    // Called by the Room 2 miniboss when it dies (wire its death event to this method in the Inspector)
    public void OnMinibossDefeated()
    {
        if (state != State.Room2Miniboss) return;
        PlayThen(minibossCutscene, OpenRoom3);
    }

    private void OpenRoom3()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(OpenRoom3);
        state = State.Room3Transition;
        if (swordsmanCompanion != null) swordsmanCompanion.gameObject.SetActive(true);
        missionUI?.SetMission(room3Header, room3Task, room3EntranceTarget);
        if (room2To3Gate != null) room2To3Gate.SetActive(false);
    }

    // Room 3 trigger that hands off to the next Act
    public void EnterRoom3()
    {
        if (state != State.Room3Transition) return;
        state = State.Done;
        SceneManager.LoadScene(nextSceneName);
    }
}
