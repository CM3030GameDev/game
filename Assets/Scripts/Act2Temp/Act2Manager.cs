using System.Collections.Generic;
using UnityEngine;

public class Act2Manager : MonoBehaviour
{
    //probably should use a list instead..
    [Header("Narrator Lines")]
    public DialogueData actStartDialogue;
    public DialogueData generatorStartDialogue;
    public DialogueData generatorDepletedDialogue;
    public DialogueData generatorDestroyedDialogue;
    

    public Generator generator;

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
/*        if(state == CurrentState.MOBHUNTING && enemies.Count <= 0 && isBossSpawned == false)
        {
            Debug.Log("state changed to BossFight");
            state = CurrentState.BOSSFIGHT;
            SpawnBoss();
            isBossSpawned = true;
        }*/

        if(state == CurrentState.MOBHUNTING && mobtest.Instance.GetKillCount() >= 2)
        {
            mobtest.Instance.StopAllSpawnCoroutines();
            //state = CurrentState.BOSSFIGHT;
            //no boss yet, skip to generator
            state = CurrentState.GENERATOR;
        }

        if(state == CurrentState.GENERATOR && hasGeneratorStarted == false)
        {
            hasGeneratorStarted = true;
            OnBossDead();
        }
    }

    //This is called when starting dialogue ends.Mobs start spawning
    private void SpawnMobs()
    {
        DialogueManager.Instance.onDialogueEnd.RemoveListener(SpawnMobs);
        state = CurrentState.MOBHUNTING;
        mobtest.Instance.AddSpawnCoroutine("mobhunting1", 1f, mobtest.EnemyTypes.REDMOB, null, 1);
        mobtest.Instance.AddSpawnCoroutine("mobhunting2", 1f, mobtest.EnemyTypes.BLUEMOB, null, 1);
    }
    private void SpawnBoss()
    {
        if (state != CurrentState.MOBHUNTING)
            return;

        //boss dialogue
        //bossPrefab.SetActive(true);

        state = CurrentState.BOSSFIGHT;
    }

    public void OnBossDead()
    {
        generator.SetIsOverdrive(true);
        state = CurrentState.GENERATOR;
        mobtest.Instance.AddSpawnCoroutine("generatorMobs1", 1f, mobtest.EnemyTypes.REDMOB);
        mobtest.Instance.AddSpawnCoroutine("generatorMobs2", 3f, mobtest.EnemyTypes.BLUEMOB);
        mobtest.Instance.AddSpawnCoroutine("generatorMobs3", 6f, mobtest.EnemyTypes.GREENMOB);
        DialogueManager.Instance.StartDialogue(generatorStartDialogue);
    }

    public void OnGeneratorShieldDown()
    {
        DialogueManager.Instance.StartDialogue(generatorDepletedDialogue);
        mobtest.Instance.StopAllSpawnCoroutines();
    }

    //maybe no shield, just let the energy deplete then self destruct, disabling(killing) all mobs alive
    public void OnGeneratorDown()
    {
        DialogueManager.Instance.StartDialogue(generatorDestroyedDialogue);
        mobtest.Instance.instantKillAllActive();
        state = CurrentState.END;
    }
}
