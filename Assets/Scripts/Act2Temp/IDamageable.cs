using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damageAmount = 0, float damageCooldown = 0f);
}
