using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invisibility : MonoBehaviour
{
    [Header("Color Settings")]
    public Color32 invisible_color;
    public Color32 visible_color;

    [Header("Invisibility Stats")]
    [SerializeField] private float invisible_damage;
    public bool is_visible;

    public GameObject flame;

    PlayerStats player_stats => GetComponent<PlayerStats>();
    SpriteRenderer player_sprite => GetComponent<SpriteRenderer>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !player_stats.is_dead)
        {
            if (is_visible)
            {
                Invisible();
            }
            else
            {
                Visible();
            }
        }
    }
    void Invisible()
    {
        is_visible = false;
        flame.SetActive(false);
        player_sprite.color = invisible_color;

        InvisibleDamage();
    }
    void Visible()
    {
        is_visible = true;
        flame.SetActive(true);
        player_sprite.color = visible_color;
    }

    void InvisibleDamage()
    {
        StartCoroutine(TakeInvisibleDamage());
    }

    IEnumerator TakeInvisibleDamage()
    {
        float player_health = GetComponent<PlayerStats>().GetPlayerHealth;

        while (!is_visible && player_health > 0)
        {
            yield return new WaitForSeconds(0.2f);
            player_stats.TakeDamage(invisible_damage);
        }
    }
}
