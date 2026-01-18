using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;
    private Vector2 move;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = move * speed;
    }
}
