using UnityEngine;

public abstract class SecondaryWeapon : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;

    protected SecondaryWeaponData data;
    protected PlayerAim playerAim;
    protected int level = 1;
    protected float timer;

    protected WeaponLevel Stats => data.levels[level - 1];
    protected int TotalDamage => Stats.damage + Mathf.RoundToInt(characterStats.damage);

    public SecondaryWeaponData Data => data;
    public int Level => level;
    public bool IsMaxLevel => level >= data.MaxLevel;

    public void Init(SecondaryWeaponData d, PlayerAim aim)
    {
        data = d; playerAim = aim; level = 1;
        OnInit();
    }

    public void LevelUp()
    {
        if (!IsMaxLevel) level++;
        OnLevelChanged();
    }

    protected virtual void OnInit() { }
    protected virtual void OnLevelChanged() { }

    protected virtual void Update()
    {
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;
        if (timer >= Stats.fireInterval)
        {
            Fire();
            timer = 0f;
        }
    }

    protected abstract void Fire();

    protected GameObject FindNearestEnemy()
    {
        GameObject nearest = null;
        float nearestDist = Stats.range;
        foreach (var e in MobManager.Instance.GetAllPooledEnemies())
        {
            if (!e.activeInHierarchy) continue;
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < nearestDist) { nearestDist = d; nearest = e; }
        }
        return nearest;
    }

    protected Vector2 Rotate(Vector2 v, float deg)
    {
        float r = deg * Mathf.Deg2Rad;
        float c = Mathf.Cos(r), s = Mathf.Sin(r);
        return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
    }
}