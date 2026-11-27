using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    public BalanceMinigameManagager minigameManagager;

    public bool enableStageTwo = false;

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
            if (enableStageTwo)
            {
                minigameManagager.EnableStageTwo();
            }

            minigameManagager.StartMinigame();
        }
    }
}
