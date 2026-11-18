using UnityEngine;

public class StartButton : MonoBehaviour
{
    public RoundManager roundManager;
    public AudioSource audioSource;

    private bool hasBeenActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenActivated)
        {
            return;
        }

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            hasBeenActivated = true;

            audioSource.Play();
            roundManager.StartRound();
        }
    }
}