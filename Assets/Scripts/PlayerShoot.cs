using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject firePrefab;
    public Transform firePoint;
    public PlayerLight playerLight;
    public float lightCost = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // คลิกซ้าย
        {
            if (playerLight.currentRadius > playerLight.minRadius + lightCost)
            {
                ShootToMouse();
                playerLight.UseLight(lightCost);
            }
        }
    }

    void ShootToMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector2 direction = mouseWorldPos - firePoint.position;

        GameObject fire = Instantiate(firePrefab, firePoint.position, Quaternion.identity);
        fire.GetComponent<FireProjectile>().SetDirection(direction);
    }
}
