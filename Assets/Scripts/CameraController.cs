
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 100f;

    public float collisionRadius = 0.3f;
    public float collisionOffset = 0.1f;
    public LayerMask collisionLayers;

    private float yaw = 0f;
    private float pitch = 20f;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * rotationSpeed * Time.deltaTime;
        pitch -= mouseY * rotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition = player.transform.position + Vector3.up * height;

        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        Vector3 desiredPosition = targetPosition + offset;

        Vector3 direction = desiredPosition - targetPosition;
        float desiredDistance = direction.magnitude;

        if (Physics.SphereCast(targetPosition, collisionRadius, direction.normalized, out RaycastHit hit, desiredDistance, collisionLayers))
        {
            desiredPosition = targetPosition + direction.normalized * Mathf.Max(hit.distance - collisionOffset, 0.1f);
        }

        transform.position = desiredPosition;
        transform.LookAt(targetPosition);
    }
}
