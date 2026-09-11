using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    //Current health
    public int health = 100;

    //Max health
    public int maxHealth = 100;

    //Experience point
    public int expPoint = 0;

    //Current level
    public int level = 1;

    //Current movement speed
    public float moveSpeed = 7f;

    //Current attack speed(Starts at 0 attack speed, capped at 0.8 attack speed)
    public float attackSpeed = 0f;

    //Flat bonus damage added to all weapon hits
    public float damage = 0f;

    //Exp orb magnet radius
    public float pickupRadius = 3f;

    //Health regenerated per second
    public float healthRegen = 0f;

    //Reset to default
    public void ResetStats()
    {
        health = 100;
        maxHealth = 100;
        expPoint = 0;
        level = 1;
        moveSpeed = 7f;
        attackSpeed = 0f;
        damage = 0f;
        pickupRadius = 3f;
        healthRegen = 0f;
    }
}
