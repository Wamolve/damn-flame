using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Status")]
    public bool isDead;
    [Header("Stats")]
    public float damage;
    public float health;
    public float armor;
    public float invisible_damage;
    public void Update()
    {
        HealthSystem();
    }
    public void HealthSystem()
    {
        if (health <= 0)
        {
            isDead = true;
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage / armor * 0.75f;
    }
    public void InvisibleDamage()
    {
        StartCoroutine(TakeInvisibleDamage());
    }
    IEnumerator TakeInvisibleDamage()
    {
        yield return new WaitForSeconds(0.2f);
        health -= invisible_damage;

        if (!GetComponent<Invisibility>().is_visible && health > 0)
        {
            StartCoroutine(TakeInvisibleDamage());
        }
    }
}
