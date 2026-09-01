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
    

    public Generator generator;
    private GameObject miniboss;
    [SerializeField] private Transform minibossSpawnPoint;
    private bool hasBossSpawned = false;

    private enum CurrentState
    {
        START,
        MOBHUNTING,
        BOSSFIGHT,
        GENERATOR,
        END
    }

    private CurrentState state = CurrentState.START;
    private bool isBossSpawned = false;
    private bool hasGeneratorStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = CurrentState.START;

        DialogueManager.Instance.StartDialogue(actStartDialogue);
        DialogueManager.Instance.onDialogueEnd.AddListener(SpawnMobs);

        generator.onGeneratorShieldDown.AddListener(OnGeneratorShieldDown);
        generator.onGeneratorDown.AddListener(OnGeneratorDown);
    }

    private void OnDestroy()
    {
        if (generator != null)
        {
            generator.onGeneratorShieldDown.RemoveListener(OnGeneratorShieldDown);
            generator.onGeneratorDown.RemoveListener(OnGeneratorDown);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(state == CurrentState.MOBHUNTING && MobManager.Instance.GetKillCount() >= 2)
        {
            MobManager.Instance.StopAllSpawnCoroutines();
            state = CurrentState.BOSSFIGHT;
        }

        if(state == CurrentState.BOSSFIGHT)
        {
            if(!hasBossSpawned)
            {
                miniboss = MobManager.Instance.SpawnBoss(MobManager.EnemyTypes.ACT2BOSS, minibossSpawnPoint);
                miniboss.GetComponent<Act2Miniboss>().bossDeath.AddListener(OnBossDeath);
                MobManager.Instance.AddSpawnCoroutine("bossfight1", 2f, MobManager.EnemyTypes.REDMOB);
                MobManager.Instance.AddSpawnCoroutine("bossfight2", 4f, MobManager.EnemyTypes.BLUEMOB);
                MobManager.Instance.AddSpawnCoroutine("bossfight3", 6f, MobManager.EnemyTypes.GREENMOB);
                hasBossSpawned = true;
                DialogueManager.Instance.StartDialogue(bossSpawnDialogue);
            }
        }
    }

    private void OnBossDeath()
    {
        DialogueManager.Instance.StartDialogue(bossDeathDialogue);
        DialogueManager.Instance.onDialogueEnd.AddListener(EnableGenerator);
        MobManager.Instance.StopAllSpawnCoroutines();
    }

    //This is called when starting dialogue ends.Mobs start spawning
    private void SpawnMobs()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(SpawnMobs);
        state = CurrentState.MOBHUNTING;
        MobManager.Instance.AddSpawnCoroutine("mobhunting1", 1f, MobManager.EnemyTypes.REDMOB, null, 1);
        MobManager.Instance.AddSpawnCoroutine("mobhunting2", 1f, MobManager.EnemyTypes.BLUEMOB, null, 1);
    }

    public void EnableGenerator()
    {
        miniboss.GetComponent<Act2Miniboss>().bossDeath.RemoveListener(OnBossDeath);
        DialogueManager.Instance.onDialogueEnd.RemoveListener(EnableGenerator);
        generator.SetIsOverdrive(true);
        state = CurrentState.GENERATOR;
        MobManager.Instance.AddSpawnCoroutine("generatorMobs1", 1f, MobManager.EnemyTypes.REDMOB);
        MobManager.Instance.AddSpawnCoroutine("generatorMobs2", 3f, MobManager.EnemyTypes.BLUEMOB);
        MobManager.Instance.AddSpawnCoroutine("generatorMobs3", 6f, MobManager.EnemyTypes.GREENMOB);
        DialogueManager.Instance.StartDialogue(generatorStartDialogue);
    }

    public void OnGeneratorShieldDown()
    {
        DialogueManager.Instance.StartDialogue(generatorDepletedDialogue);
        MobManager.Instance.StopAllSpawnCoroutines();
    }

    //maybe no shield, just let the energy deplete then self destruct, disabling(killing) all mobs alive
    public void OnGeneratorDown()
    {
        DialogueManager.Instance.StartDialogue(generatorDestroyedDialogue);
        MobManager.Instance.instantKillAllActive();
        state = CurrentState.END;
    }
}
