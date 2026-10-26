using UnityEngine;

public class RLGLCeiling : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 5f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;

    void Start()
    {
        startPos = pointA.position;
        endPos = pointB.position;
        transform.position = startPos;
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);

        if (transform.position == endPos)
            isMoving = false;
    }

    public void StartMovement()
    {
        Debug.Log($"{name} StartMovement called: {startPos} -> {endPos}");
        transform.position = startPos;
        isMoving = true;
    }
}