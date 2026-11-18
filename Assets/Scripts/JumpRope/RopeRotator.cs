using UnityEngine;

public class RopeRotator : MonoBehaviour
{
    public float baseSpeed = -100f;
    public float speedVariation = 0.5f;

    private float lastZ;

    void Start()
    {
        lastZ = transform.eulerAngles.z;
    }

    void Update()
    {
        float zRotation = transform.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f;

        float rotationMultiplier = 1f + Mathf.Sin(zRotation * Mathf.Deg2Rad) * speedVariation;
        transform.Rotate(Vector3.forward * baseSpeed * rotationMultiplier * Time.deltaTime);

        float currentZ = transform.eulerAngles.z;
        if (currentZ > 180f) currentZ -= 360f;

        bool isSwingingDown = currentZ < lastZ;

        lastZ = currentZ;
    }
}
