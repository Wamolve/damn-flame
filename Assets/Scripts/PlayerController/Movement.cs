using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walk_speed;
    public float run_speed;
    public bool is_run;

    private float current_speed;
    private bool is_facing_right = true;

    [Header("Jump Settings")]
    public float jump_force;
    public float ground_check_radius;
    public LayerMask ground_layer;
    public Transform ground_check;
    
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    public GameObject swordCollider;
    public float swordActiveTime = 0.3f;
    private float lastAttackTime;
    private bool isAttacking = false;
    
    [Header("Animations")]
    public Animator anim;
    
    private Rigidbody2D rb;
    private bool is_grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, ground_check_radius, ground_layer);

        current_speed = is_run ? run_speed : walk_speed;

        HandleMovementInput();
        HandleJumpInput();
        HandleRunInput();
        HandleAttackInput();

        UpdateAnimations();

        if(isAttacking && Time.time > lastAttackTime + swordActiveTime)
        {
            EndAttack();
        }
    }
    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        swordCollider.SetActive(true);
    }

    void EndAttack()
    {
        isAttacking = false;
        swordCollider.SetActive(false);
    }

    void HandleMovementInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput < 0)
        {
            MoveLeft();
            if (is_facing_right) Flip();
        }
        else if (moveInput > 0)
        {
            MoveRight();
            if (!is_facing_right) Flip();
        }
        else
        {
            is_run = false;
        }
    }

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && is_grounded)
            Jump();
    }

    void HandleRunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            is_run = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            is_run = false;
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            StartAttack();
            lastAttackTime = Time.time;
        }
    }

    void MoveLeft()
    {
        transform.position += Vector3.left * current_speed * Time.deltaTime;
        is_run = true;
    }

    void MoveRight()
    {
        transform.position += Vector3.right * current_speed * Time.deltaTime;
        is_run = true;
    }

    void Flip()
    {
        is_facing_right = !is_facing_right;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
        anim.SetTrigger("Jump");
    }


    void UpdateAnimations()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        anim.SetBool("IsGrounded", is_grounded);
        anim.SetBool("IsRunning", is_run);
        anim.SetBool("IsIdle", !is_run && is_grounded);
        anim.SetFloat("VerticalVelocity", rb.velocity.y);
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