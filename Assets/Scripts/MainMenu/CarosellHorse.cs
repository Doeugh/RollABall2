using UnityEngine;

public class CarosellHorse : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    public float speed = 5f;

    private Transform target;

    void Start()
    {
        transform.position = pointA.position;

        target = pointB;
    }

    void Update()
    {
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
