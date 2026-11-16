using UnityEngine;

public class RopeColider : MonoBehaviour
{
    public float launchForce = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;

                Vector3 launchDirection = transform.right;

                launchDirection.y = 0;

                rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
            }
        }
    }
}
