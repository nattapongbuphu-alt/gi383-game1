using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    [Header("Movement Settings")]
    public LayerMask wallLayer;
    public float normalSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float detectRadius = 4f;
    public float wanderTime = 2f;
    public float wallCheckDistance = 0.5f;
    
    [Header("Attack Settings")]
    public float damageLight = 1f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private Vector2 wanderDirection;
    private bool isStunned = false;
    private bool canAttack = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Wander());
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 moveDir;

        if (CanSeePlayer())
        {
            moveDir = (player.position - transform.position).normalized;

            // ถ้ามีกำแพงข้างหน้า → หยุดไล่ชั่วคราว
            if (IsWallAhead(moveDir))
            {
                moveDir = wanderDirection;
            }

            rb.linearVelocity = moveDir * chaseSpeed;
        }
        else
        {
            if (IsWallAhead(wanderDirection))
            {
                wanderDirection = Random.insideUnitCircle.normalized;
            }

            rb.linearVelocity = wanderDirection * normalSpeed;
        }
    }

    bool CanSeePlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > detectRadius) return false;

        RaycastHit2D hit = Physics2D.Linecast(
            transform.position,
            player.position,
            wallLayer
        );

        // ถ้าโดนกำแพงก่อน = มองไม่เห็น
        return hit.collider == null;
    }

    IEnumerator Wander()
    {
        while (true)
        {
            wanderDirection = Random.insideUnitCircle.normalized;
            yield return new WaitForSeconds(wanderTime);
        }
    }

    // 🔥 ถูกยิง → สตั้น
    public void Stun(float stunTime)
    {
        if (!isStunned)
            StartCoroutine(StunCoroutine(stunTime));
    }

    IEnumerator StunCoroutine(float time)
    {
        isStunned = true;
        yield return new WaitForSeconds(time);
        isStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            PlayerLight playerLight = collision.gameObject.GetComponent<PlayerLight>();
            if (playerLight != null)
            {
                playerLight.TakeDamage(damageLight);
            }

            Destroy(gameObject);
            //StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    bool IsWallAhead(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            wallCheckDistance,
            wallLayer
        );

        Debug.DrawRay(transform.position, dir * wallCheckDistance, Color.yellow);

        return hit.collider != null;
    }
}
