using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float health;
    public float armor;
    public void HealthSystem()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage / armor*0.75f;
    }
}
