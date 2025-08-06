using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsUpdate : MonoBehaviour
{
    public Animator player_animator;

    public void PlayerAnimationsUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        MovementSystem movement = gameObject.GetComponent<MovementSystem>();
        Stats stats = gameObject.GetComponent<Stats>();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        player_animator.SetBool("IsGrounded", movement.is_grounded);
        player_animator.SetBool("IsRunning", movement.is_run);
        player_animator.SetBool("IsIdle", !movement.is_run && movement.is_grounded);
        player_animator.SetFloat("VerticalVelocity", rb.velocity.y);
        player_animator.SetFloat("VerticalVelocity", rb.velocity.y);
        player_animator.SetBool("isWalk", !movement.is_run && movement.is_walk);
        player_animator.SetBool("isDead", stats.isDead);
    }
}