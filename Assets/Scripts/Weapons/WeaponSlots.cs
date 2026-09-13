using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponSlots : MonoBehaviour
{
    [SerializeField] private int maxSlots = 3; // secondary weapons only
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Image[] weaponSlotImages; // index 0 = main, 1-3 = secondary
    [SerializeField] private LevelPips[] weaponSlotPips; // same indexing as weaponSlotImages
    [SerializeField] private const int secondaryOffset = 1; // Reserves first slot for the main weapon

    private readonly List<SecondaryWeapon> active = new List<SecondaryWeapon>();
    public bool IsFull => active.Count >= maxSlots;
    public IReadOnlyList<SecondaryWeapon> Active => active;

    // Evolves an owned weapon into its combined form. Pips stay full since it was already maxed.
    public void Combine(SecondaryWeapon w, SecondaryWeaponData combined)
    {
        int i = active.IndexOf(w);
        if (i < 0) return;

        w.Combine(combined);
        SetSlotIcon(i + secondaryOffset, combined.icon);
        MarkCombined(i + secondaryOffset);
    }

    private void MarkCombined(int index)
    {
        if (index < 0 || index >= weaponSlotPips.Length || weaponSlotPips[index] == null) return;
        weaponSlotPips[index].SetCombined(true);
    }

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

    public bool AcquireOrLevel(SecondaryWeaponData d)
    {
        SecondaryWeapon existing = Find(d);
        if (existing != null)
        {
            existing.LevelUp();
            RefreshPips(active.IndexOf(existing) + secondaryOffset, existing.Level);
            return true;
        }
        if (IsFull) return false;

        if (d.weaponPrefab == null)
        {
            Debug.LogError($"{d.name} has no Weapon Prefab assigned - it cannot be equipped.", d);
            return false;
        }

        int index = active.Count + secondaryOffset;
        GameObject go = Instantiate(d.weaponPrefab, transform.position,
                                    Quaternion.identity, transform);
        SecondaryWeapon w = go.GetComponent<SecondaryWeapon>();
        w.Init(d, playerAim);
        active.Add(w);

        SetSlotIcon(index, d.icon);
        RefreshPips(index, w.Level);
        return true;
    }

    // Used when a loadout is restored and the weapon's level was set without going through
    // AcquireOrLevel for each step.
    public void SetSlotLevel(SecondaryWeapon w)
    {
        int i = active.IndexOf(w);
        if (i < 0) return;
        SetSlotIcon(i + secondaryOffset, w.Data.icon);
        RefreshPips(i + secondaryOffset, w.Level);
    }

    public void SetSlotIcon(int index, Sprite icon)
    {
        if (index < 0 || index >= weaponSlotImages.Length) return;
        weaponSlotImages[index].sprite = icon;
        weaponSlotImages[index].preserveAspect = true;
        weaponSlotImages[index].color = Color.white;
    }

    public void SetMainWeaponIcon(Sprite icon) => SetSlotIcon(0, icon);
    public void SetMainWeaponLevel(int level) => RefreshPips(0, level);

    private void RefreshPips(int index, int level)
    {
        if (index < 0 || index >= weaponSlotPips.Length || weaponSlotPips[index] == null) return;
        weaponSlotPips[index].SetLevel(level);
    }
}