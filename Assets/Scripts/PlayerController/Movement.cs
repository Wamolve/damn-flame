using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walk_speed;
    public float run_speed;
    public bool is_run;

    private float current_speed;

    [Header("Jump Settings")]
    public float jump_force;
    public float ground_check_radius;
    public LayerMask ground_layer;
    public Transform ground_check;
    
    private Rigidbody2D rb;
    private bool is_grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, ground_check_radius, ground_layer);

        if (is_run)
        {
            current_speed = run_speed;
        }
        else
        {
            current_speed = walk_speed;
        }

        if (Input.GetKey(KeyCode.A)) 
            MoveLeft();

        if (Input.GetKey(KeyCode.D))
            MoveRight();

        if (Input.GetKeyDown(KeyCode.Space) && is_grounded)
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            is_run = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            is_run = false;
    }

    void MoveLeft()
    {
        transform.position = transform.position + new Vector3(-1, 0, 0) * current_speed * Time.deltaTime;
    }

    void MoveRight()
    {
        transform.position = transform.position + new Vector3(1, 0, 0) * current_speed * Time.deltaTime;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        if (ground_check != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ground_check.position, ground_check_radius);
        }
    }
}
