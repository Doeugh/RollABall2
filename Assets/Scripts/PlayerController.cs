using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text winText;

    public float speed = 0;
    public float jumpForce = 5f;
    public float maxSpeed = 0;

    public Transform cameraTransform;

    private Rigidbody rb;

    private int PickupCount;

    private float movementX;
    private float movementY;

    private bool isGrounded;

    void Start()
    {
        PickupCount = 0;
        SetCountText();

        rb = GetComponent<Rigidbody>();
        winText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if (jumpValue.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = camForward * movementY + camRight * movementX;

        rb.AddForce(movement * speed, ForceMode.Acceleration);

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;

            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
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
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            PickupCount++;
            SetCountText();
            other.gameObject.SetActive(false);
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + PickupCount.ToString();

        if (PickupCount >= 4)
        {
            winText.gameObject.SetActive(true);
        }
    }
}