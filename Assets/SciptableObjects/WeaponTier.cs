using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTier", menuName = "Scriptable Objects/WeaponTier")]
public class WeaponTier : ScriptableObject
{
    public string tierName = "Pistol";
    public GameObject bulletPrefab;
    public int damage = 20;
    public float bulletSpeed = 50f;
    public float fireInterval = 1f;
    public float spreadAngle = 0f;   // random spread Pistol

    [Tooltip("Muzzle points bullets emit from. 1 point = single gun, 2 = alternating dual.")]
    public Vector2[] muzzleOffsets = new Vector2[] { new Vector2(0f, 0.12f) };
}