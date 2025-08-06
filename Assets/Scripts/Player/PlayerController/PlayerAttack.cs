using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        float damage = player.GetComponent<Stats>().damage;

        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
}
