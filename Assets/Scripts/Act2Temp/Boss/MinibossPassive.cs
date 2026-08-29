using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MinibossPassive : MonoBehaviour
{
    [Header("Passive Settings")]
    [SerializeField] private GameObject minionObject;
    [SerializeField] private int numberOfMinions = 5;
    [SerializeField] private int gridSpacing = 5;
    [SerializeField] private float spawnInterval = 1f;

    [Header("Minion Settings")]
    [SerializeField] private float growthSpeed = 20f;
    [SerializeField] private float maxLength = 150f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int damage = 5;

    public UnityEvent completedSpawning;
    private bool isSpawning = false;
    public void TriggerSpawn()
    {
        if (isSpawning) return;
            StartCoroutine(SpawnMinions());
    }

    private IEnumerator SpawnMinions()
    {
        isSpawning = true;

        GameObject player = GameObject.FindGameObjectsWithTag("Character")[0];
        Vector2 playerPos = player.transform.position;
        float topOffset = (numberOfMinions - 1) * gridSpacing / 2f;
        float sideOffset = (numberOfMinions - 1) * gridSpacing / 2f;

        for (int col = 1; col <= numberOfMinions; col++)
        {
            float xOffset = (col - 1) * gridSpacing - topOffset;
            GameObject y = Instantiate(minionObject, playerPos + new Vector2(xOffset, topOffset), Quaternion.identity);
            y.GetComponent<Minion>().Initialize(Minion.DirectionFacing.TOP, moveSpeed, growthSpeed, damage, maxLength);
            yield return new WaitForSeconds(spawnInterval);
        }

        for (int row = 1; row <= numberOfMinions; row++)
        {
            float yOffset = (row * gridSpacing) - sideOffset;
            GameObject x = Instantiate(minionObject, playerPos + new Vector2((-sideOffset) - gridSpacing, -yOffset), Quaternion.identity);
            x.GetComponent<Minion>().Initialize(Minion.DirectionFacing.LEFT, moveSpeed, growthSpeed, damage, maxLength);
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
        completedSpawning?.Invoke();
    }

    public bool GetIsSpawning()
    {
        return isSpawning;
    }
}
