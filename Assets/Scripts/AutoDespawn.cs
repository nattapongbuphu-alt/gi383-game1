using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 30f;

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
