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

        // ������ա������͹����������
        if (movement.x != 0 || movement.y != 0)
        {
            // �ѻവ��ȷҧ੾�е͹������
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);

            // �觤�� Speed �� 1 ���������蹷���Թ
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            // �ҡ��衴������� (Idle) ����觤�� Speed �� 0 �ѹ��
            animator.SetFloat("Speed", 0f);
        }
    }

    void FixedUpdate()
    {
        // ���������Ф�����͹���
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}