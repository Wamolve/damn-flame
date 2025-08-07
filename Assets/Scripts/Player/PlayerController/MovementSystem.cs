using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walk_speed;
    [SerializeField] private float run_speed;

    public bool is_run;
    public bool is_walk;

    private float current_speed;
    private bool is_facing_right;

    [Header("Jump Settings")]
    [SerializeField] private float jump_force;
    [SerializeField] private float ground_check_radius;
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private Transform ground_check;

    public bool is_grounded;

    private AnimationsUpdate update_animations => GetComponent<AnimationsUpdate>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    

    void Update()
    {
        current_speed = is_run ? run_speed : walk_speed;

        is_grounded = Physics2D.OverlapCircle(ground_check.position, ground_check_radius, ground_layer);

        if (!GetComponent<PlayerStats>().is_dead)
        {
            Movement();
            Jump();
            Run();
        }

        UpdateAnimations();
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