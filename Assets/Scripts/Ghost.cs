using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    public float normalSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float detectRadius = 4f;
    public float wanderTime = 2f;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 wanderDirection;
    private bool isStunned = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(Wander());
    }

    void FixedUpdate()
    {
        if (isStunned)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // ถ้าเจอผู้เล่น → ไล่
        if (distance <= detectRadius)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.linearVelocity = dir * chaseSpeed;
        }
        else
        {
            rb.linearVelocity = wanderDirection * normalSpeed;
        }
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
}
