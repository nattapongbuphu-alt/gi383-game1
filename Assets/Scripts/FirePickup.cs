using UnityEngine;

public class FirePickup : MonoBehaviour
{
    public float lightAmount = 0.8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLight playerLight = other.GetComponent<PlayerLight>();
            if (playerLight != null)
            {
                playerLight.AddLight(lightAmount);
            }

            Destroy(gameObject);
        }
    }
}
