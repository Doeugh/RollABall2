using UnityEngine;

public class RopeSound : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rope"))
        {
            audioSource.Play();
        }
    }
}
