using UnityEngine;
using System.Collections.Generic;

public class WeaponSlots : MonoBehaviour
{
    [SerializeField] private int maxSlots = 3;
    [SerializeField] private Mobs mobs;
    [SerializeField] private PlayerAim playerAim;

    private readonly List<SecondaryWeapon> active = new List<SecondaryWeapon>();

    public bool IsFull => active.Count >= maxSlots;

    public SecondaryWeapon Find(SecondaryWeaponData d)
    {
        foreach (var w in active) if (w.Data == d) return w;
        return null;
    }

    public bool CanOffer(SecondaryWeaponData d)
    {
        SecondaryWeapon existing = Find(d);
        if (existing != null) return !existing.IsMaxLevel;
        return !IsFull;
    }

    public void AcquireOrLevel(SecondaryWeaponData d)
    {
        SecondaryWeapon existing = Find(d);
        if (existing != null) { existing.LevelUp(); return; }
        if (IsFull) return;

        GameObject go = Instantiate(d.weaponPrefab, transform.position,
                                    Quaternion.identity, transform);
        SecondaryWeapon w = go.GetComponent<SecondaryWeapon>();
        w.Init(d, mobs, playerAim);
        active.Add(w);
    }
}