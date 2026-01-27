/* using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody2D rb;
    Vector2 move;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move = move.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }
}*/
