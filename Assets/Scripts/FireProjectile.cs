using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float stunTime = 2f;
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ghost"))
        {
            Ghost ghost = other.GetComponent<Ghost>();
            if (ghost != null)
            {
                ghost.Stun(stunTime); // 👻 โดนสตั้น
            }

            Destroy(gameObject); // กระสุนหาย
        }
    }
}
