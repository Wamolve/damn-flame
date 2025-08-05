using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float speed = 3f;
    public float damage = 10f;
    public float health = 100f;
    public float armor = 1f;


    [Header("Enemy Target")]
    public GameObject detectedPlayer;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    private bool canAttack = true;

    [Header("Enemy Physics")]
    private Rigidbody2D rb;
    
    [Header("Detection Settings")]
    public float detectionDistance = 5f;
    public LayerMask detectionMask;
    public float raycastOffset = 0.5f;

    [Header("Patrol Settings")]
    public float patrolDistance = 3f;
    private Vector2 startPosition;
    private bool movingRight = true;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    private void Update()
    {
        HealthSystem();
        DetectPlayer();
        
        if (detectedPlayer == null)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
            TryAttackPlayer();
        }
    }

    private void HealthSystem()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage / (armor * 0.7f);
    }

    private void DetectPlayer()
    {
        // Проверяем игрока с обеих сторон врага
        RaycastHit2D hitRight = Physics2D.Raycast(
            new Vector2(transform.position.x + raycastOffset, transform.position.y),
            isFacingRight ? Vector2.right : Vector2.left,
            detectionDistance,
            detectionMask);

        RaycastHit2D hitLeft = Physics2D.Raycast(
            new Vector2(transform.position.x - raycastOffset, transform.position.y),
            isFacingRight ? Vector2.right : Vector2.left,
            detectionDistance,
            detectionMask);

        if (hitRight.collider != null && hitRight.collider.CompareTag("Player"))
        {
            detectedPlayer = hitRight.collider.gameObject;
        }
        else if (hitLeft.collider != null && hitLeft.collider.CompareTag("Player"))
        {
            detectedPlayer = hitLeft.collider.gameObject;
        }
        else
        {
            detectedPlayer = null;
        }
    }

    private void Patrol()
    {
        // Движение вправо
        if (movingRight)
        {
            if (transform.position.x < startPosition.x + patrolDistance)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                movingRight = false;
                Flip();
            }
        }
        // Движение влево
        else
        {
            if (transform.position.x > startPosition.x - patrolDistance)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            else
            {
                movingRight = true;
                Flip();
            }
        }
    }

    private void ChasePlayer()
    {
        if (detectedPlayer == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, detectedPlayer.transform.position);

        // Если игрок вне зоны атаки - преследуем
        if (distanceToPlayer > attackRange)
        {
            float direction = Mathf.Sign(detectedPlayer.transform.position.x - transform.position.x);
            rb.velocity = new Vector2(direction * speed, rb.velocity.y);

            // Поворачиваем врага в сторону игрока
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            {
                Flip();
            }
        }
        else
        {
            // Останавливаемся если в зоне атаки
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void TryAttackPlayer()
    {
        if (detectedPlayer == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, detectedPlayer.transform.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer()
    {
        canAttack = false;
        
        // Наносим урон игроку
        PlayerStats playerHealth = detectedPlayer.GetComponent<PlayerStats>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Визуализация зон обнаружения и атаки в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        Gizmos.color = Color.yellow;
        Vector3 rayStart = transform.position;
        rayStart.x += isFacingRight ? raycastOffset : -raycastOffset;
        Gizmos.DrawLine(rayStart, rayStart + (isFacingRight ? Vector3.right : Vector3.left) * detectionDistance);
    }
}