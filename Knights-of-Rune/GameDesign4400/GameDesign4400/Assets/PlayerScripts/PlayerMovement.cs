using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement Speed
    public float baseSpeed = 3f;
    public float sprintSpeed = 3f;
    private float currentSpeed = 3f;

    Vector2 moveInput; // cached per-frame input

    // Map Variables
    public Camera mainCamera;
    public Camera mapCamera;
    private Vector3 mapScale = new Vector3(15f, 15f, 15f);
    private Vector3 normalScale = new Vector3(5f, 5f, 1f);
    private bool mapOpen = false;

    // Collisions
    public Rigidbody2D rb;


    private void Start()
    {
        mainCamera.enabled = true;
        mapCamera.enabled = false;
    }

    void Update()
    {
        // Read input every frame so you don't miss taps
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;

        // attack

        // sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = baseSpeed + sprintSpeed;
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        // map
        if (Input.GetKey(KeyCode.M))
        {
            mapOpen = true;
            transform.localScale = mapScale; 
            mainCamera.enabled = false;
            mapCamera.enabled = true;
        }
        else
        {
            mapOpen = false;
            transform.localScale = normalScale;
            mainCamera.enabled = true;
            mapCamera.enabled = false;
        }



    }

    void FixedUpdate()
    {
        if (mapOpen == false)
        {
            // Apply movement on the fixed physics step
            Vector2 targetPos = rb.position + moveInput * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }
}
