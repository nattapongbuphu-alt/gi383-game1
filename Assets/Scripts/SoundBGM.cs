using UnityEngine;

public class SoundBGM : MonoBehaviour
{
    public AudioClip bgmMusic; // Background music
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Play background music on loop
        if (bgmMusic != null)
        {
            audioSource.clip = bgmMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
