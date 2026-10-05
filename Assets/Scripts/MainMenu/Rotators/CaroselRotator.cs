using UnityEngine;

public class CaroselRotator : MonoBehaviour
{
    public float speed = 50f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
