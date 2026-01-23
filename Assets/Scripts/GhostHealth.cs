using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    public float maxHP = 3f;
    private float currentHP;

    [Header("Drop Settings")]
    public GameObject lightDropPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.7f; // 70%

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        DropLight();
        Destroy(gameObject);
    }

    void DropLight()
    {
        if (lightDropPrefab == null) return;

        if (Random.value <= dropChance)
        {
            Instantiate(
                lightDropPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}
