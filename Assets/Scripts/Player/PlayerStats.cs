using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Status")]
    public bool is_dead;

    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private float armor;

    private float difficulty_factor = 0.75f;

    public float GetPlayerDamage
    {
        get { return damage; }
        set { damage = value; }
    }
    public float GetPlayerHealth
    {
        get { return health; }
        set { health = value; }
    }
    public float GetPlayerArmor
    {
        get { return armor; }
        set { armor = value; }
    }

    void Update()
    {
        if (health <= 0)
        {
            is_dead = true;
            health = 0;
        }
    }
    public void TakeDamage(float take_damage)
    {
        health -= take_damage / (armor * difficulty_factor);
    }
}
