using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invisibility : MonoBehaviour
{
    [Header("Invisibility Settings")]
    public Color32 invisible_color = new Color32(0, 136, 173, 100);
    public GameObject flame;
    public SpriteRenderer player_sprite;

    public bool is_visible;
    
    void Start()
    {
        player_sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
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
    void Invisible()
    {
        is_visible = false;
        flame.SetActive(false);
        player_sprite.color = invisible_color;
        GetComponent<Stats>().InvisibleDamage();
    }
    void Visible()
    {
        is_visible = true;
        flame.SetActive(true);
        player_sprite.color = new Color32(255, 255, 255, 255);
        is_visible = true;
    }
}
