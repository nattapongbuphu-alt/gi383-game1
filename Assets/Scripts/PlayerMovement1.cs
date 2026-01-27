using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // เช็คว่ามีการเคลื่อนที่หรือไม่
        if (movement.x != 0 || movement.y != 0)
        {
            // อัปเดตทิศทางเฉพาะตอนกดปุ่ม
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);

            // ส่งค่า Speed เป็น 1 เพื่อให้เล่นท่าเดิน
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            // หากไม่กดปุ่มเลย (Idle) ให้ส่งค่า Speed เป็น 0 ทันที
            animator.SetFloat("Speed", 0f);
        }
    }

    void FixedUpdate()
    {
        // สั่งให้ตัวละครเคลื่อนที่
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}