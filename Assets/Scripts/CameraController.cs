using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 100f;

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

        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = player.transform.position + offset + Vector3.up * height;

        transform.LookAt(player.transform.position + Vector3.up * height);
    }
}