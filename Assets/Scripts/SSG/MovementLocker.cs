using UnityEngine;

public class MovementLocker : MonoBehaviour
{
    public RoundManager roundManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.canMove = false;
            }
        }

        if (other.CompareTag("Enemy"))
        {
            SSGEnemyAI enemy = other.GetComponent<SSGEnemyAI>();

            if (enemy != null)
            {
                enemy.canMove = false;
                roundManager.RegisterFall(other.gameObject);
            }
        }

        SSGEnemyAI targetAI = other.GetComponent<SSGEnemyAI>();

        if (targetAI != null)
        {
            targetAI.isTargetable = false;
        }
    }
}
