using UnityEngine;

public class FirePickup : MonoBehaviour
{
    public float lightAmount = 0.8f;
    public AudioSource PickUp_Source;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLight playerLight = other.GetComponent<PlayerLight>();
            if (playerLight != null)
            {
                playerLight.AddLight(lightAmount);
            }

            // เล่นเสียง PickUp Effect
            if (PickUp_Source != null)
            {
                PickUp_Source.PlayOneShot(PickUp_Source.clip);
            }

            Destroy(gameObject, 0.1f);
        }
    }
}
