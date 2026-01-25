using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float stunTime = 2f;
    public float lifeTime = 5f;
    private Vector2 direction;
    private float timeAlive = 0f;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        
        // นับเวลาและทำลายลูกไฟหลังจากเวลาที่กำหนด
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ghost"))
        {
            GhostHealth ghost = collision.GetComponent<GhostHealth>();
            if (ghost != null)
            {
                ghost.TakeDamage(1f); // ดาเมจต่อนัด
            }

            Destroy(gameObject); // ลูกไฟหาย
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // ลูกไฟชนกำแพงแล้วทำลายตัวเอง
        }
    }

}
