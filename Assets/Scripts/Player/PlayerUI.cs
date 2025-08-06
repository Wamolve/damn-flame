using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player UI")]
    public Stats player_stats;
    public float player_health;
    public Slider player_slider;
    public void Start()
    {
        player_stats = GetComponent<Stats>();
        player_health = player_stats.health;
    }

    public void Update()
    {
        player_health = player_stats.health;
        player_slider.value = player_health;
    }
}
