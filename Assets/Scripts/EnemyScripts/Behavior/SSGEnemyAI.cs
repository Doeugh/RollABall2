using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SSQEnemyMovement))]
public class SSGEnemyAI : MonoBehaviour
{
    public Transform target;
    public float targetSwitchInterval = 6f;

    public bool canMove = false;

    public float retreatDistance = 8f;
    public float retreatCheckDistance = 3f;

    public float chargeDuration = 2f;

    public float edgeCheckDistance = 1.5f;
    public LayerMask groundLayer;

    public float brakingForce = 8f;
    public float minimumSpeedToTurn = 1f;

    private SSQEnemyMovement movement;
    private Rigidbody rb;

    private enum AIState
    {
        Retreating,
        Charging
    }

    private AIState currentState = AIState.Retreating;

    private float chargeTimer;
    private float targetSwitchTimer;

    private Vector3 retreatDirection;

    void Awake()
    {
        movement = GetComponent<SSQEnemyMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        SelectRandomTarget();
        ChooseRetreatDirection();

        targetSwitchTimer = targetSwitchInterval;
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            movement.StopMovement();
            return;
        }

        targetSwitchTimer -= Time.fixedDeltaTime;

        if (targetSwitchTimer <= 0f)
        {
            SelectRandomTarget();
            targetSwitchTimer = targetSwitchInterval;
        }

        if (target == null)
        {
            movement.StopMovement();
            return;
        }

        switch (currentState)
        {
            case AIState.Retreating:
                Retreat();
                break;

            case AIState.Charging:
                Charge();
                break;
        }
    }

    void SelectRandomTarget()
    {
        List<Transform> possibleTargets = new List<Transform>();

        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController player in players)
        {
            if (player != null && player.transform != transform)
            {
                possibleTargets.Add(player.transform);
            }
        }

        SSGEnemyAI[] enemies = FindObjectsByType<SSGEnemyAI>(FindObjectsSortMode.None);

        foreach (SSGEnemyAI enemy in enemies)
        {
            if (enemy != null && enemy.transform != transform)
            {
                if (!possibleTargets.Contains(enemy.transform))
                {
                    possibleTargets.Add(enemy.transform);
                }
            }
        }

        List<Transform> alternativeTargets = new List<Transform>(possibleTargets);

        alternativeTargets.Remove(target);

        if (alternativeTargets.Count > 0)
        {
            possibleTargets = alternativeTargets;
        }

        if (possibleTargets.Count > 0)
        {
            target = possibleTargets[Random.Range(0, possibleTargets.Count)];

            currentState = AIState.Retreating;
            chargeTimer = 0f;

            ChooseRetreatDirection();
        }
    }

    void Retreat()
    {
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.magnitude >= retreatDistance)
        {
            currentState = AIState.Charging;
            chargeTimer = chargeDuration;
            return;
        }

        Vector3 checkPosition =
            transform.position + retreatDirection * edgeCheckDistance;

        if (!HasGround(checkPosition))
        {
            Vector3 horizontalVelocity = new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                brakingForce * Time.fixedDeltaTime
            );

            rb.linearVelocity = new Vector3(
                horizontalVelocity.x,
                rb.linearVelocity.y,
                horizontalVelocity.z
            );

            if (horizontalVelocity.magnitude <= minimumSpeedToTurn)
            {
                ChooseRetreatDirection();
            }

            return;
        }

        movement.Move(retreatDirection);
    }

    void Charge()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        movement.Move(direction);

        chargeTimer -= Time.fixedDeltaTime;

        if (chargeTimer <= 0f)
        {
            currentState = AIState.Retreating;
            ChooseRetreatDirection();
        }
    }

    void ChooseRetreatDirection()
    {
        Vector3 awayFromTarget = transform.position - (target != null ? target.position : transform.position + transform.forward);

        awayFromTarget.y = 0f;

        if (awayFromTarget.sqrMagnitude < 0.01f)
        {
            awayFromTarget = -transform.forward;
        }

        awayFromTarget.Normalize();

        Vector3[] directions =
        {
            awayFromTarget,
            Quaternion.Euler(0, 30, 0) * awayFromTarget,
            Quaternion.Euler(0, -30, 0) * awayFromTarget,
            Quaternion.Euler(0, 60, 0) * awayFromTarget,
            Quaternion.Euler(0, -60, 0) * awayFromTarget,
            Quaternion.Euler(0, 90, 0) * awayFromTarget,
            Quaternion.Euler(0, -90, 0) * awayFromTarget,
            -awayFromTarget
        };

        foreach (Vector3 direction in directions)
        {
            Vector3 checkPosition = transform.position + direction * edgeCheckDistance;

            if (HasGround(checkPosition))
            {
                retreatDirection = direction.normalized;
                return;
            }
        }

        retreatDirection = Vector3.zero;
    }

    bool HasGround(Vector3 position)
    {
        return Physics.Raycast(position + Vector3.up * 2f, Vector3.down, 5f, groundLayer);
    }
}
