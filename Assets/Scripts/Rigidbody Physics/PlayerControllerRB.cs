using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRB : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float moveSpeed = 1f;
    Vector3 forceDirection;
    Vector3 xzInput;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        forceDirection = (((xzInput.x * transform.right) + (xzInput.z * transform.forward)) * moveSpeed)
            + (forceDirection.y * transform.up);
        rb.AddForce(forceDirection * moveSpeed, ForceMode.Force);
    }

    public void RBMove(InputAction.CallbackContext value)
    {
        Vector2 inputValue = value.ReadValue<Vector2>();
        xzInput = new Vector3(inputValue.x, 0f, inputValue.y);
    }
}
