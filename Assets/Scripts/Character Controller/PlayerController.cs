using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    private Vector3 xzInput;
    private Vector3 velocity;

    [Header("Player Controls Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityValue;

    private float coyoteTime = 0.3f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [Header("Ground Detection Hookups")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    private float groundDistance = 0.2f;

    private bool grounded;
    private bool canJump;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        velocity = (((xzInput.x * transform.right) + (xzInput.z * transform.forward)) * moveSpeed) + (velocity.y * transform.up);

        if (canJump || (grounded && jumpBufferCounter > 0))
        {
            jumpBufferCounter = 0; // Prevents unintended additional jumps
            coyoteTimeCounter = 0; // Prevents double jump coyote time bug
            velocity.y = jumpForce;
            canJump = false;
        }
        else if (grounded && velocity.y < 0)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFallingWithoutJumping", false);
            jumpBufferCounter = 0;
            coyoteTimeCounter = coyoteTime;
            velocity.y = -5f; // Prevents gravity from forcing player down too quickly if they drop off a ledge
        }
        else
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFallingWithoutJumping", true);
            jumpBufferCounter -= Time.deltaTime;
            coyoteTimeCounter -= Time.deltaTime;
            velocity.y -= gravityValue * Time.deltaTime; 
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        Vector2 inputValue = value.ReadValue<Vector2>();
        xzInput = new Vector3(inputValue.x, 0f, inputValue.y);

        if (value.performed && !animator.GetBool("IsJumping"))
            animator.SetBool("IsRunning", true);
        else if (value.canceled)
            animator.SetBool("IsRunning", false);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (coyoteTimeCounter > 0f)
            {
                // Jump state reached
                animator.SetBool("IsJumping", true);
                canJump = true;
            }
            else
                jumpBufferCounter = jumpBufferTime;
        }
    }
}
