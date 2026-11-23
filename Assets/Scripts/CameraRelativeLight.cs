using UnityEngine;

public class CameraRelativeLight : MonoBehaviour
{
    public Transform cameraTransform;
    public Light directionalLight;

    [Range(0f, 80f)]
    public float lightAngle = 25f;

    void LateUpdate()
    {
        if (cameraTransform == null || directionalLight == null)
            return;

        // Get the camera's horizontal rotation only.
        float cameraYaw = cameraTransform.eulerAngles.y;

        // Keep the light at a fixed vertical angle.
        directionalLight.transform.rotation =
            Quaternion.Euler(lightAngle, cameraYaw, 0f);
    }
}
