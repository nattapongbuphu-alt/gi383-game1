using UnityEngine;

public class MonsterWalkDown : MonoBehaviour
{
    [Header("Animation Settings")]
    public float walkDuration = 3f;
    public float idleDuration = 2f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator anim;

    private float timer;
    private bool isWalking;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (anim == null) anim = GetComponent<Animator>();
        StartWalking();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (isWalking) 
                StartIdle();
            else 
                StartWalking();
        }

        if (anim != null)
        {
            float currentSpeed = isWalking ? 1f : 0f;
            anim.SetFloat("Speed", currentSpeed);
        }
    }


    void StartWalking()
    {
        isWalking = true;
        timer = walkDuration;
    }

    void StartIdle()
    {
        isWalking = false;
        timer = idleDuration;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}