using UnityEngine;

public class CarosellHorse : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    public float speed = 5f;
    public float delay = 2f;

    private Transform target;
    private float delayTimer;

    void Start()
    {
        transform.position = pointA.position;

        target = pointB;
        delayTimer = delay;
    }

    void Update()
    {
        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position == target.position)
        {
            if (target == pointB)
            {
                target = pointC;
            }
            else
            {
                target = pointB;
            }
        }
    }
}
