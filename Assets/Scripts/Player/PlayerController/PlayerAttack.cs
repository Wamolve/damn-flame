using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject player;
    public GameObject sword_collider;

    [Header("Attack Settings")]
    [SerializeField] private float attack_cooldown;
    [SerializeField] private float sword_active_time;
    [SerializeField] private float last_attack_time;

    private bool is_attacking;

    private AnimationsUpdate update_animations => player.GetComponent<AnimationsUpdate>();

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (!GetComponent<PlayerStats>().is_dead) 
            Attack();

        if (is_attacking && Time.time > last_attack_time + sword_active_time) 
            EndAttack();
    }

    void OnTriggerEnter2D(Collider2D col) //доделать атаку игрока(т.к. скрипт висит теперь на игроке, данный кусок не работает)
    {
        float damage = player.GetComponent<PlayerStats>().GetPlayerDamage;

        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > last_attack_time + attack_cooldown)
        {
            StartAttack();
            last_attack_time = Time.time;
        }
    }

    void StartAttack()
    {
        is_attacking = true;
        last_attack_time = Time.time;
        update_animations.player_animator.SetTrigger("Attack");
        sword_collider.SetActive(true);
    }

    void EndAttack()
    {
        is_attacking = false;
        sword_collider.SetActive(false);
    }
}
