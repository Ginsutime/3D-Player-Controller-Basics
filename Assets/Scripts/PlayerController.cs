using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    private Vector3 xzInput;
    private Vector3 velocity;

    [Header("Player Controls Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityValue;
    private float coyoteTime = 0.3f;
    private float coyoteTimeCounter;

    [Header("Ground Detection Hookups")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    private float groundDistance = 0.2f;

    private bool grounded;
    private bool canJump;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Smoother in Update (Use fixed if you have colliders that move)
    // Also, geometry tested by raycast refreshed only after FixedUpdate
    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        velocity = (((xzInput.x * transform.right) + (xzInput.z * transform.forward)) * moveSpeed) + (velocity.y * transform.up);

        // Find way to prevent gravity from forcing player down too quickly if they drop off a ledge
        if (canJump)
        {
            coyoteTimeCounter = 0;
            velocity.y = jumpForce;
            canJump = false;
        }
        else if (grounded && velocity.y < 0)
        {
            coyoteTimeCounter = coyoteTime;
            velocity.y = -5f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            velocity.y -= gravityValue * Time.deltaTime; 
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 inputValue = value.ReadValue<Vector2>();
        xzInput = new Vector3(inputValue.x, 0f, inputValue.y);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (coyoteTimeCounter > 0f)
            {
                Debug.Log("Jump state reached");
                canJump = true;
            }
        }
    }
}
