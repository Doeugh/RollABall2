using UnityEngine;

public class StartButton : MonoBehaviour
{
    public RoundManager roundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            roundManager.StartRound();
        }
    }
}
