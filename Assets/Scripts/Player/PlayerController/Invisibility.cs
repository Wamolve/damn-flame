using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invisibility : MonoBehaviour
{
    public bool is_visible;
    public GameObject flame;
    public SpriteRenderer player_sprite;
    public void Start()
    {
        player_sprite = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !GetComponent<Stats>().isDead)
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
    public void Invisible()
    {
        is_visible = false;
        flame.SetActive(false);
        player_sprite.color = new Color(1, 1, 1, 50);
        GetComponent<Stats>().InvisibleDamage();
    }
    public void Visible()
    {
        is_visible = true;
        flame.SetActive(true);
        player_sprite.color = new Color(1, 1, 1, 255);
        is_visible = true;
    }
}
