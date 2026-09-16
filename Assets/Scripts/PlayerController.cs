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

    public Transform cameraTransform;

    private Rigidbody rb;

    private int PickupCount;

    private float movementX;
    private float movementY;

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

    void FixedUpdate()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 movement = camForward * movementY + camRight * movementX;

        rb.AddForce(movement * speed);

        if (movement.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    10f * Time.deltaTime
                )
            );
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
        if(PickupCount >= 4)
        {
            winText.gameObject.SetActive(true);
        }
    }

}