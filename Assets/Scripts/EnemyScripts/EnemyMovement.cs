using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SSQEnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 15f;
    public float maxSpeed = 6f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            return;
        }

        direction.Normalize();

        rb.AddForce(direction * speed, ForceMode.Acceleration);

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;

            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }

    public void StopMovement()
    {
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        rb.angularVelocity = Vector3.zero;
    }
}

