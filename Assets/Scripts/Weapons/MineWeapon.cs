using UnityEngine;
using System.Collections.Generic;

public class MineWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private int mineCap = 10;                      // hard ceiling at every rank
    [SerializeField] private float combinedSecondBlastDelay = 3f;   // combined form only

    private readonly List<GameObject> mines = new List<GameObject>();

    protected override void Fire()
    {
        mines.RemoveAll(m => m == null);   // clear spent mines

        int maxMines = Mathf.Min(Mathf.RoundToInt(Stats.valueA), mineCap);
        if (mines.Count >= maxMines) return;

        GameObject m2 = Instantiate(minePrefab, transform.position, Quaternion.identity);

        Mine mine = m2.GetComponent<Mine>();
        if (mine != null)
            mine.Configure(TotalDamage, Stats.valueB, IsCombined, combinedSecondBlastDelay);   // valueB = blast radius

        mines.Add(m2);
    }
}
