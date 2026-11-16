using UnityEngine;

public class MovementLocker : MonoBehaviour
{
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
            }
        }
    }
}
