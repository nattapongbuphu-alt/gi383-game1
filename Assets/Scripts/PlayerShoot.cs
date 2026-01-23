using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float lightCost = 0.5f;

    private PlayerLight playerLight;

    void Start()
    {
        playerLight = GetComponent<PlayerLight>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryShoot();
        }
    }

    void TryShoot()
    {
        // 🔦 เช็กแสงก่อนยิง
        if (!playerLight.HasEnoughLight(lightCost))
        {
            return; // แสงไม่พอ ยิงไม่ได้
        }

        Shoot();
        playerLight.TakeDamage(lightCost);
    }

    void Shoot()
    {
        // ตำแหน่งเมาส์ในโลก
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ทิศทางยิง (2D ล้วน)
        Vector2 direction = (mouseWorldPos - firePoint.position);

        // สร้างลูกไฟ
        GameObject fireball = Instantiate(
            fireballPrefab,
            firePoint.position,
            Quaternion.identity
        );

        // ยิง
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * bulletSpeed;
    }
}
