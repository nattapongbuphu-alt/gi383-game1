using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource SFX_Source;
    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // ตรวจสอบว่าผู้เล่นกำลังเดินหรือไม่
        if (movement.x != 0 || movement.y != 0)
        {
            // กำหนดทิศทางที่ตัวละคร
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);

            // ตั้งค่า Speed เป็น 1 เพื่อให้ Animation เล่นเดิน
            animator.SetFloat("Speed", 1f);

            // เล่นเสียง Walking Effect
            if (!SFX_Source.isPlaying)
            {
                SFX_Source.Play();
            }
        }
        else
        {
            // หยุด Animation (Idle) และตั้งค่า Speed เป็น 0 ทันที
            animator.SetFloat("Speed", 0f);

            // หยุดเสียง Walking Effect
            if (SFX_Source.isPlaying)
            {
                SFX_Source.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        // ���������Ф�����͹���
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}