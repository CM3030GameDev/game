using UnityEngine;
using UnityEngine.SceneManagement;

// Act1Manager (this is to manage missions, enemy spawns etc)
public class Act1Manager : MonoBehaviour
{
    // Room2Intro exists so Update ignores the kill/level gate while the entry dialogue plays,
    // which would otherwise re-show the mission panel over the cutscene.
    // Room2Intro exists so Update ignores the kill/level gate while the entry dialogue plays.
    // Room 3 is two stages: walk to the waiting swordsman, then move out together.
    private enum State { Room1Travel, Room2Intro, Room2Secure, Room2Miniboss,
                         Room3Travel, Room3Meeting, Room3Escort, Done }
    private State state;

    [Header("References")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MissionUI missionUI;

    [Header("Audio")]
    [Tooltip("Index into UIAudioManager's Background Music list. -1 plays nothing.")]
    [SerializeField] private int actMusicIndex = -1;

    [Header("Opening Dialogue")]
    [SerializeField] private DialogueData openingDialogue; // leave this empty if you guys want to skip straight to Room 1
    [SerializeField] private DialogueData room2EntryDialogue; // plays on entering Room 2, before the waves start

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
    [SerializeField] private Companion swordsmanCompanion; // stays hidden until he agrees to come along
    [Tooltip("A plain sprite standing in Room 3. Shown while he is waiting, then swapped for the " +
             "real companion at the same position so there is never one of each on screen.")]
    [SerializeField] private GameObject swordsmanStandIn;

    [Header("Room 3")]
    [SerializeField] private DialogueData swordsmanDialogue;  // he agrees to come along and names the route
    [SerializeField] private DialogueData actEndDialogue;     // last line before Act 2 loads
    [SerializeField] private string room3EscortTask = "Follow the swordsman's route deeper into the city.";
    [SerializeField] private Transform actExitTarget;         // arrow target once the swordsman joins

    [Header("Act Transition")]
    [SerializeField] private string nextSceneName = "Act4";
    [Tooltip("Card shown on the black screen while the next act loads.")]
    [SerializeField] private string nextActTitle = "Act 2 - The City";
    [SerializeField] private MenuSceneTransition sceneTransition;

    private void Start()
    {
        if (actMusicIndex >= 0 && UIAudioManager.Instance != null)
            UIAudioManager.Instance.PlayBGM(actMusicIndex, true);

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
        state = State.Room2Intro;

        MobManager.Instance.StopSpawnCoroutine(Room1WaveName);
        killsAtRoom2Start = MobManager.Instance.GetKillCount();
        if (room1To2Gate != null) room1To2Gate.SetActive(false);

        PlayThen(room2EntryDialogue, BeginRoom2Waves);
    }

    private void BeginRoom2Waves()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(BeginRoom2Waves);
        state = State.Room2Secure;
        missionUI?.SetMission(room2Header, BuildRoom2Tasks(), null);

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
        state = State.Room3Travel;

        // The stand-in does the waiting. The companion has follow AI, a collider and an animator,
        // none of which should run until he actually joins.
        if (swordsmanStandIn != null) swordsmanStandIn.SetActive(true);

        missionUI?.SetMission(room3Header, room3Task, room3EntranceTarget);
        if (room2To3Gate != null) room2To3Gate.SetActive(false);
    }

    // Room 3 trigger that hands off to the next Act
    // Reaching the swordsman waiting in Room 3
    public void EnterRoom3()
    {
        if (state != State.Room3Travel) return;
        state = State.Room3Meeting;
        PlayThen(swordsmanDialogue, SwordsmanJoins);
    }

    private void SwordsmanJoins()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(SwordsmanJoins);
        state = State.Room3Escort;

        // Hand over at the stand-in's position, so the companion appears exactly where the
        // player was just talking to him rather than popping in from wherever he was parked.
        if (swordsmanCompanion != null)
        {
            if (swordsmanStandIn != null)
                swordsmanCompanion.transform.position = swordsmanStandIn.transform.position;

            swordsmanCompanion.gameObject.SetActive(true);
            swordsmanCompanion.enabled = true;
        }
        if (swordsmanStandIn != null) swordsmanStandIn.SetActive(false);

        missionUI?.SetMission(room3Header, room3EscortTask, actExitTarget);
    }

    // The exit further up, past the swordsman
    public void EnterExtraction()
    {
        if (state != State.Room3Escort) return;
        state = State.Done;
        PlayThen(actEndDialogue, LoadNextAct);
    }

    private void LoadNextAct()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(LoadNextAct);

        if (sceneTransition != null) sceneTransition.LoadSceneWithFade(nextSceneName, nextActTitle);
        else SceneManager.LoadScene(nextSceneName);   // no transition wired: still leave the act
    }
}
