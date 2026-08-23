using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponSlots : MonoBehaviour
{
    [SerializeField] private int maxSlots = 3; // secondary weapons only
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Image[] weaponSlotImages; // index 0 = main, 1-3 = secondary
    [SerializeField] private const int secondaryOffset = 1; // Reserves first slot for the main weapon

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

    public bool AcquireOrLevel(SecondaryWeaponData d)
    {
        SecondaryWeapon existing = Find(d);
        if (existing != null)
        {
            existing.LevelUp();
            return true;
        }
        if (IsFull) return false;

        int index = active.Count + secondaryOffset;
        GameObject go = Instantiate(d.weaponPrefab, transform.position,
                                    Quaternion.identity, transform);
        SecondaryWeapon w = go.GetComponent<SecondaryWeapon>();
        w.Init(d, playerAim);
        active.Add(w);

        SetSlotIcon(index, d.icon);
        return true;
    }

    public void SetSlotIcon(int index, Sprite icon)
    {
        if (index < 0 || index >= weaponSlotImages.Length) return;
        weaponSlotImages[index].sprite = icon;
        weaponSlotImages[index].color = Color.white;
    }

    public void SetMainWeaponIcon(Sprite icon) => SetSlotIcon(0, icon);
}