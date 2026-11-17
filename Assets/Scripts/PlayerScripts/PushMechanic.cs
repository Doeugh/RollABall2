using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushMechanic : MonoBehaviour
{
    public float knockbackMultiplier = 1.5f;
    public float minimumImpactSpeed = 2f;
    public float upwardForce = 0.5f;

    public bool affectPlayers = true;
    public bool affectEnemies = true;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody;

        if (otherRb == null || otherRb == rb)
        {
            return;  
        }

        bool isPlayer = collision.gameObject.CompareTag("Player");
        bool isEnemy = collision.gameObject.CompareTag("Enemy");

        if (isPlayer && !affectPlayers)
        {
            return;
        }

        if (isEnemy && !affectEnemies)
        {
            return;
        }

        if (!isPlayer && !isEnemy)
        {
            return;
        }

        Vector3 impactVelocity = rb.linearVelocity;
        impactVelocity.y = 0f;

        float impactSpeed = impactVelocity.magnitude;

        if (impactSpeed < minimumImpactSpeed)
        {
            return;
        }

        Vector3 knockbackDirection = impactVelocity.normalized;

        knockbackDirection.y = upwardForce;

        knockbackDirection.Normalize();

        Vector3 knockback = knockbackDirection * impactSpeed * knockbackMultiplier;

        otherRb.AddForce(knockback, ForceMode.Impulse);
    }
}
