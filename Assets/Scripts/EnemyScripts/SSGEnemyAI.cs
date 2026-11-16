using UnityEngine;

[RequireComponent(typeof(SSQEnemyMovement))]
public class SSGEnemyAI : MonoBehaviour
{
    public Transform target;

    public bool canMove = false;

    private SSQEnemyMovement movement;

    void Awake()
    {
        movement = GetComponent<SSQEnemyMovement>();
    }

    void Start()
    {
        if (target == null)
        {
            PlayerController player = FindFirstObjectByType<PlayerController>();

            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void FixedUpdate()
    {
        if (!canMove || target == null)
        {
            movement.StopMovement();
            return;
        }

        Vector3 direction = target.position - transform.position;

        direction.y = 0f;

        movement.Move(direction);
    }
}
