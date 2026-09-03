using System.Collections.Generic;
using UnityEngine;

public class Act2Manager : MonoBehaviour
{
    [Header("Narrator Lines")]
    public DialogueData actStartDialogue;

    public DialogueData bossSpawnDialogue;
    public DialogueData bossDeathDialogue;

    public DialogueData generatorStartDialogue;
    public DialogueData generatorDepletedDialogue;
    public DialogueData generatorDestroyedDialogue;

    [System.Serializable]
    public struct EnemySpawnSetting
    {
        public string name;
        public MobManager.EnemyTypes enemyType;
        public int spawnCount;
        public float spawnInterval;
        public int minWave;
        public int maxWave;
    }

    [Header("References")]
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MissionUI missionUI;


    [Header("Room 1 - Kill Count")]
    private int room1TargetKillCount;
    [SerializeField] private List<EnemySpawnSetting> room1SpawnerList = new List<EnemySpawnSetting>();
    private const string Room1WaveName = "room1_killcount";

    [Header("Room 2 Part 1 - Boss")]
    [SerializeField] private List<EnemySpawnSetting> room2SpawnerList1 = new List<EnemySpawnSetting>();

    [Header("Room 2 Part 2 - Generator")]
    [SerializeField] private List<EnemySpawnSetting> room2SpawnerList2 = new List<EnemySpawnSetting>();

    [Header("Mission Text - edit wording here, not in code")]
    [SerializeField] private string room1Header = "Act 2 - Push Deeper Into the City";
    [SerializeField] private string room1Task = "Clear the area.";
    [SerializeField] private string room1ClearedTask = "The next area has opened. Head deeper into the city.";

    [SerializeField] private string room2Header1 = "Act 2 - Defeat the boss";
    [SerializeField] private string room2Task1 = "Defeat the boss.";
    [SerializeField] private string room2ClearedTask1 = "Boss has been taken down";

    [SerializeField] private string room2Header2 = "Act 2 - Survive the waves.";
    [SerializeField] private string room2Task2 = "Act 2 - Stay alive until the generator runs out of energy";

    [SerializeField] private string room2Header3 = "Act 2 - Destroy the generator.";
    [SerializeField] private string room2Task3 = "Act 2 - Destroy the generator";
    [SerializeField] private string room2ClearedTask3 = "Generator has been destroyed.";

    [Header("Objective Targets (for the direction arrow)")]
    [SerializeField] private Transform room2EntranceTarget;
    [SerializeField] private Transform act3EntranceTarget;
    [SerializeField] private Transform generatorTarget;

    [Header("Gates (physically block the way until each stage opens up)")]
    [SerializeField] private GameObject room1ToBossGate;
    [SerializeField] private GameObject BossToAct3Gate;

    public Generator generator;
    private GameObject miniboss;
    [SerializeField] private Transform minibossSpawnPoint;
    private bool hasBossSpawned = false;
    private bool isRoom1Cleared = false;

    private enum CurrentState
    {
        START,
        ROOM1,
        BOSSFIGHT,
        GENERATOR,
        END
    }

    private CurrentState state = CurrentState.START;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = CurrentState.START;

        //Enable starting dialogue, and an even listener to begin the act when dialogue ends
        DialogueManager.Instance.StartDialogue(actStartDialogue);
        DialogueManager.Instance.onDialogueEnd.AddListener(SpawnRoom1Mobs);

        //Event listeners for the generator
        generator.onGeneratorShieldDown.AddListener(OnGeneratorShieldDown);
        generator.onGeneratorDown.AddListener(OnGeneratorDown);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == CurrentState.ROOM1 && MobManager.Instance.GetKillCount() >= room1TargetKillCount)
        {
            if (!isRoom1Cleared)
            {
                MobManager.Instance.StopAllSpawnCoroutines();
                isRoom1Cleared = true;
                //Mission UI
                missionUI?.SetMission(room1Header, room1ClearedTask, room2EntranceTarget);
            }
            else
            {

            }
            WaitForRoomEnter();

            
        }

        if(state == CurrentState.BOSSFIGHT)
        {
            if(!hasBossSpawned)
            {
                //Spawn boss and add boss death listener
                miniboss = MobManager.Instance.SpawnBoss(MobManager.EnemyTypes.ACT2BOSS, minibossSpawnPoint);
                miniboss.GetComponent<Act2Miniboss>().bossDeath.AddListener(OnBossDeath);

                //Add mob spawn coroutines
                foreach (EnemySpawnSetting setting in room2SpawnerList1)
                {
                    if (setting.minWave > 0 && setting.maxWave > 0)
                    {
                        MobManager.Instance.AddWaveSpawnCoroutine(setting.name, setting.enemyType, setting.minWave, setting.maxWave, setting.spawnInterval);
                    }
                    else
                        MobManager.Instance.AddSpawnCoroutine(setting.name, setting.spawnInterval, setting.enemyType, setting.spawnCount);
                }
                hasBossSpawned = true;

                //Enable boss spawn dialogue
                DialogueManager.Instance.StartDialogue(bossSpawnDialogue);

                //Mission UI
                missionUI?.SetMission(room2Header1, room2Task1, room2EntranceTarget);
            }
        }

        if(state == CurrentState.END)
        {
            //open gate
        }
    }

    private void WaitForRoomEnter()
    {
        state = CurrentState.BOSSFIGHT;
    }
    private void OnBossDeath()
    {
        //Enable boss death dialogue
        DialogueManager.Instance.StartDialogue(bossDeathDialogue);

        //Mission UI
        missionUI?.SetMission(room2Header1, room2ClearedTask1, room2EntranceTarget);

        //Remove this function's listener
        miniboss.GetComponent<Act2Miniboss>().bossDeath.RemoveListener(OnBossDeath);

        //Event listener to enable generator after boss death dialogue and stop all mob spawners
        DialogueManager.Instance.onDialogueEnd.AddListener(EnableGenerator);
        MobManager.Instance.StopAllSpawnCoroutines();
    }

    //This is called when starting dialogue ends. Mobs start spawning
    private void SpawnRoom1Mobs()
    {
        //Mission UI
        missionUI?.SetMission(room1Header, room1Task, room2EntranceTarget);

        //Remove this function's listener 
        DialogueManager.Instance.onDialogueEnd.RemoveListener(SpawnRoom1Mobs);

        state = CurrentState.ROOM1;

        //Add mob spawn coroutines
        foreach (EnemySpawnSetting setting in room1SpawnerList)
        {
            if (setting.minWave > 0 && setting.maxWave > 0)
            {
                MobManager.Instance.AddWaveSpawnCoroutine(setting.name, setting.enemyType, setting.minWave, setting.maxWave, setting.spawnInterval);
            }
            else
            {
                MobManager.Instance.AddSpawnCoroutine(setting.name, setting.spawnInterval, setting.enemyType, setting.spawnCount);
                room1TargetKillCount += setting.spawnCount;
            }
        }
    }

    public void EnableGenerator()
    {
        //Despawn the dead boss and remove this function's listener
        MobManager.Instance.DespawnBoss(MobManager.EnemyTypes.ACT2BOSS);
        DialogueManager.Instance.onDialogueEnd.RemoveListener(EnableGenerator);

        //Mission UI
        missionUI?.SetMission(room2Header2, room2Task2, room2EntranceTarget);

        generator.SetIsOverdrive(true);
        state = CurrentState.GENERATOR;

        MobManager.Instance.AddSpawnCoroutine("generatorMobs1", 1f, MobManager.EnemyTypes.REDMOB);
        MobManager.Instance.AddSpawnCoroutine("generatorMobs2", 3f, MobManager.EnemyTypes.BLUEMOB);
        MobManager.Instance.AddSpawnCoroutine("generatorMobs3", 6f, MobManager.EnemyTypes.GREENMOB);

        //Enable generator start dialogue
        DialogueManager.Instance.StartDialogue(generatorStartDialogue);
    }

    public void OnGeneratorShieldDown()
    {
        //Mission UI
        missionUI?.SetMission(room2Header3, room2Task3, room2EntranceTarget);

        //Enable generator depleted dialogue and stop all spawners
        DialogueManager.Instance.StartDialogue(generatorDepletedDialogue);
        MobManager.Instance.StopAllSpawnCoroutines();

        //Remove this function's listener
        generator.onGeneratorShieldDown.RemoveListener(OnGeneratorShieldDown);
    }

    public void OnGeneratorDown()
    {
        //Mission UI
        missionUI?.SetMission(room2Header3, room2ClearedTask3, room2EntranceTarget);

        //Enable generator destroyed dialogue and kill all active mobs
        DialogueManager.Instance.StartDialogue(generatorDestroyedDialogue);
        MobManager.Instance.instantKillAllActive();

        state = CurrentState.END;

        //Remove this function's listener
        generator.onGeneratorDown.RemoveListener(OnGeneratorDown);
    }
}
