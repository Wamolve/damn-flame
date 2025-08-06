using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walk_speed;
    public float run_speed;
    public bool is_run;
    public bool is_walk;

    private float current_speed;
    private bool is_facing_right;

    [Header("Jump Settings")]
    public float jump_force;
    public float ground_check_radius;
    public LayerMask ground_layer;
    public Transform ground_check;
    
    [Header("Attack Settings")]
    public float attack_cooldown;
    public GameObject sword_collider;
    public float sword_active_time;
    private float last_attack_time;
    private bool is_attacking;

    private AnimationsUpdate update_animations;

    private Rigidbody2D rb;
    public bool is_grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        update_animations = GetComponent<AnimationsUpdate>();
    }

    void Update()
    {
        current_speed = is_run ? run_speed : walk_speed;

        is_grounded = Physics2D.OverlapCircle(ground_check.position, ground_check_radius, ground_layer);
        if (!GetComponent<Stats>().isDead)
        {
            Movement();
            Jump();
            Run();
            Attack();
        }

        UpdateAnimations();

        if (is_attacking && Time.time > last_attack_time + sword_active_time)
        {
            EndAttack();
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

    void Movement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput < 0)
        {
            MoveLeft();
            if (!is_facing_right) Flip();
            is_walk = true;
        }
        else if (moveInput > 0)
        {
            MoveRight();
            if (is_facing_right) Flip();
            is_walk = true;
        }
        else
        {
            is_walk = false;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && is_grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
        }
    }

    void Run()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) is_run = true;
        if (Input.GetKeyUp(KeyCode.LeftShift)) is_run = false;

    }

    void MoveLeft()
    {
        transform.position += Vector3.left * current_speed * Time.deltaTime;
    }

    void MoveRight()
    {
        transform.position += Vector3.right * current_speed * Time.deltaTime;
    }

    void Flip()
    {
        is_facing_right = !is_facing_right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void UpdateAnimations()
    {
        update_animations.PlayerAnimationsUpdate();
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