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
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
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