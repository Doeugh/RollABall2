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
    public float edgeSafetyBuffer = 0.5f;
    public LayerMask groundLayer;

    public float brakingForce = 8f;
    public float minimumSpeedToTurn = 1f;

    public bool isTargetable = true;

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

        if (HandleEdgeSafety())
        {
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

    float GetPredictiveCheckDistance()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.y = 0f;

        float speed = velocity.magnitude;

        float stoppingDistance = 0f;

        if (brakingForce > 0f)
        {
            stoppingDistance = (speed * speed) / (2f * brakingForce);
        }

        return edgeCheckDistance + stoppingDistance + edgeSafetyBuffer;
    }

    bool IsDirectionSafe(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return true;

        direction.Normalize();

        float checkDistance = GetPredictiveCheckDistance();

        Vector3 checkPosition =
            transform.position + direction * checkDistance;

        return HasGround(checkPosition);
    }

    void BrakeHorizontalMovement()
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
    }

    Vector3 GetIntendedDirection()
    {
        if (currentState == AIState.Charging && target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0f;

            return direction.normalized;
        }

        if (currentState == AIState.Retreating)
        {
            return retreatDirection.normalized;
        }

        return Vector3.zero;
    }

    bool HandleEdgeSafety()
    {
        Vector3 intendedDirection = GetIntendedDirection();

        Vector3 velocityDirection = new Vector3(
            rb.linearVelocity.x,
            0f,
            rb.linearVelocity.z
        );

        if (velocityDirection.sqrMagnitude > 0.01f)
        {
            velocityDirection.Normalize();
        }

        bool intendedDirectionSafe =
            IsDirectionSafe(intendedDirection);

        bool velocityDirectionSafe =
            velocityDirection.sqrMagnitude < 0.01f ||
            IsDirectionSafe(velocityDirection);

        if (intendedDirectionSafe && velocityDirectionSafe)
        {
            return false;
        }

        currentState = AIState.Retreating;
        chargeTimer = 0f;

        BrakeHorizontalMovement();

        ChooseRetreatDirection();

        return true;
    }

    void SelectRandomTarget()
    {
        List<Transform> possibleTargets = new List<Transform>();

        PlayerController[] players =
            FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController player in players)
        {
            if (player != null && player.transform != transform)
            {
                possibleTargets.Add(player.transform);
            }
        }

        SSGEnemyAI[] enemies =
            FindObjectsByType<SSGEnemyAI>(FindObjectsSortMode.None);

        foreach (SSGEnemyAI enemy in enemies)
        {
            if (enemy != null &&
                enemy.transform != transform &&
                enemy.isTargetable)
            {
                if (!possibleTargets.Contains(enemy.transform))
                {
                    possibleTargets.Add(enemy.transform);
                }
            }
        }

        possibleTargets.RemoveAll(t =>
        {
            if (t == null)
                return true;

            SSGEnemyAI enemyAI = t.GetComponent<SSGEnemyAI>();

            return enemyAI != null && !enemyAI.isTargetable;
        });

        List<Transform> alternativeTargets =
            new List<Transform>(possibleTargets);

        alternativeTargets.Remove(target);

        if (alternativeTargets.Count > 0)
        {
            possibleTargets = alternativeTargets;
        }

        if (possibleTargets.Count > 0)
        {
            target = possibleTargets[
                Random.Range(0, possibleTargets.Count)
            ];

            currentState = AIState.Retreating;
            chargeTimer = 0f;

            ChooseRetreatDirection();
        }
        else
        {
            target = null;
            movement.StopMovement();
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

        if (retreatDirection.sqrMagnitude < 0.01f)
        {
            ChooseRetreatDirection();

            if (retreatDirection.sqrMagnitude < 0.01f)
            {
                movement.StopMovement();
                return;
            }
        }

        movement.Move(retreatDirection);
    }

    void Charge()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            movement.StopMovement();
            return;
        }

        direction.Normalize();

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
        Vector3 awayFromTarget =
            transform.position -
            (target != null
                ? target.position
                : transform.position + transform.forward);

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
            if (IsDirectionSafe(direction))
            {
                retreatDirection = direction.normalized;
                return;
            }
        }

        retreatDirection = Vector3.zero;
    }

    bool HasGround(Vector3 position)
    {
        return Physics.Raycast(
            position + Vector3.up * 2f,
            Vector3.down,
            5f,
            groundLayer
        );
    }
}
