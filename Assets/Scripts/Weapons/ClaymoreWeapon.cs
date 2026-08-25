using UnityEngine;
using System.Collections.Generic;

public class ClaymoreWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject minePrefab;

    private readonly List<GameObject> mines = new List<GameObject>();

    protected override void Fire()
    {
        mines.RemoveAll(m => m == null);   // clear detonated mines

        int maxMines = Mathf.RoundToInt(Stats.valueA);
        if (mines.Count >= maxMines) return;

        GameObject m2 = Instantiate(minePrefab, transform.position, Quaternion.identity);
        Mine mine = m2.GetComponent<Mine>();
        if (mine != null) mine.Configure(TotalDamage, Stats.valueB);   // valueB = blast radius
        mines.Add(m2);
    }
}