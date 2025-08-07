using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsUpdate : MonoBehaviour
{
    public Animator player_animator => GetComponent<Animator>();

    public void PlayerAnimationsUpdate()
    {
        MovementSystem movement = GetComponent<MovementSystem>();
        PlayerStats stats = GetComponent<PlayerStats>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        player_animator.SetBool("IsGrounded", movement.is_grounded);
        player_animator.SetBool("IsRunning", movement.is_run);
        player_animator.SetBool("IsIdle", !movement.is_run && movement.is_grounded);
        player_animator.SetBool("isWalk", !movement.is_run && movement.is_walk);
        player_animator.SetBool("isDead", stats.is_dead);

        player_animator.SetFloat("VerticalVelocity", rb.velocity.y);
        player_animator.SetFloat("VerticalVelocity", rb.velocity.y);
    }
}