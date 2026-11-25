using UnityEngine;

public class JRMinigameFallDetector : MonoBehaviour
{
    public BalanceMinigameManagager minigameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            minigameManager.PlayerFellOff();
        }
    }
}
