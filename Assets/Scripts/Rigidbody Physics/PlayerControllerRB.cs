using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRB : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Player Controls Values")]
    [SerializeField] private float maxSpeed = 5f; // Get this hooked up eventually to cap acceleration
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float normGravityAmount = 1f;
    [SerializeField] private float lowJumpGravityAmount = 1f;
    Vector3 forceDirection;
    Vector3 xzInput;

    [Header("Ground Detection Hookups")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    private float groundDistance = 0.2f;
    private bool grounded;
    private bool canJump, releasedJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Using this over raycast in case of sloped surfaces
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        forceDirection = (((xzInput.x * transform.right) + (xzInput.z * transform.forward)) * moveSpeed)
            + (forceDirection.y * transform.up);

        if (canJump)
        {
            forceDirection.y = jumpForce;
            canJump = false;
        }
        else if (rb.velocity.y > 0 && releasedJump)
        {
            forceDirection.y -= lowJumpGravityAmount * Time.fixedDeltaTime;
        }
        else if (grounded && forceDirection.y < 0)
        {
            releasedJump = false;
            forceDirection.y = -1f; // Prevents gravity from forcing player down too quickly if they drop off a ledge
        }
        else
        {
            forceDirection.y -= normGravityAmount * Time.fixedDeltaTime;
        }

        rb.AddForce(forceDirection * moveSpeed, ForceMode.Force);
    }

    public void RBMove(InputAction.CallbackContext value)
    {
        Vector2 inputValue = value.ReadValue<Vector2>();
        xzInput = new Vector3(inputValue.x, 0f, inputValue.y);
    }

    public void RBJump(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (grounded && forceDirection.y < 0)
            {
                Debug.Log("Jump action reached");
                canJump = true;
            }
        }
        else if (value.canceled && forceDirection.y > 0)
        {
            Debug.Log("Stopped holding jump");
            releasedJump = true;
        }
    }
}
