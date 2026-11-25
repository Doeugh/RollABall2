using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    public BalanceMinigameManagager minigameManagager;

    private static bool hasBeenEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (!hasBeenEntered)
        {
            hasBeenEntered = true;
            return;
        }

        if (minigameManagager != null)
        {
            minigameManagager.StartMinigame();
        }
    }
}