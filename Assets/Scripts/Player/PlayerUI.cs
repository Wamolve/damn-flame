using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private float player_health;
    public Slider player_slider;

    void Update()
    {
        player_health = GetComponent<PlayerStats>().GetPlayerHealth;
        player_slider.value = player_health;
    }
}
