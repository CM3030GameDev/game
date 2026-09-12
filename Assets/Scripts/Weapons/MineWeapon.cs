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

        int maxMines = Mathf.Max(1, Mathf.Min(Mathf.RoundToInt(Stats.valueA), mineCap));

        // At the cap, retire the oldest instead of refusing to place. A mine you laid a minute
        // ago is worth less than one under the enemy in front of you.
        while (mines.Count >= maxMines)
        {
            GameObject oldest = mines[0];
            mines.RemoveAt(0);
            if (oldest != null) Destroy(oldest);
        }

        GameObject m2 = Instantiate(minePrefab, transform.position, Quaternion.identity);

        Mine mine = m2.GetComponent<Mine>();
        if (mine != null)
            mine.Configure(TotalDamage, Stats.valueB, IsCombined, combinedSecondBlastDelay);   // valueB = blast radius

        mines.Add(m2);
    }
}
