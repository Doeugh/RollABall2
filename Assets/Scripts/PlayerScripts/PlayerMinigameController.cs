
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMinigameController : MonoBehaviour
{
    public float speed = 0;
    public float jumpForce = 5f;
    public float maxSpeed = 0;

    private Rigidbody rb;

    private float movementX;
    private float movementY;

    private bool isGrounded;

    public bool canMove = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnFreeze(InputValue freezeValue)
    {
        if (freezeValue.isPressed)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnJump(InputValue jumpValue)
    {
        if (!canMove)
        {
            return;
        }

        if (jumpValue.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            rb.angularVelocity = Vector3.zero;
            return;
        }

        Vector3 movement = new Vector3(movementX, 0f, movementY);

        movement = Vector3.ClampMagnitude(movement, 1f);

        rb.AddForce(movement * speed, ForceMode.Acceleration);

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;

            rb.linearVelocity = new Vector3( horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }

        if (movement.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            PauseMenuScript.TogglePause();
        }
    }
}
