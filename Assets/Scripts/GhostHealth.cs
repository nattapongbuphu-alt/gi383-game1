using UnityEngine;

public class GhostHealth : MonoBehaviour
{
    public float maxHP = 3f;
    private float currentHP;
    private bool killedByShot = false;

    [Header("Drop Settings")]
    public GameObject lightDropPrefab;
    [Range(0f, 1f)]
    public float dropChance = 0.7f; // 70%

    void Start()
    {
        currentHP = maxHP;
        killedByShot = false;
    }

    // default: not from shot
    public void TakeDamage(float damage)
    {
        TakeDamage(damage, false);
    }

    // call with byShot=true when damage comes from player's projectile
    public void TakeDamage(float damage, bool byShot)
    {
        currentHP -= damage;
        if (byShot) killedByShot = true;

        if (currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        DropLight();
        if (killedByShot)
        {
            var counter = FindObjectOfType<KillCounter>();
            if (counter != null)
                counter.AddKill();
        }
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
