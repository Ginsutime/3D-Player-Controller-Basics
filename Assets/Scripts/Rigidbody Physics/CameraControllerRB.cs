using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerRB : MonoBehaviour
{
    [Header("Mouse Controls")]
    [SerializeField] private float mouseSensitivity = 1f;
    private float xRot, yRot;

    [Header("Camera Hookups/Settings")]
    [SerializeField] private Transform target;
    [SerializeField] float distanceOffset = 3f;

    [SerializeField] private float smoothTime = 0.2f;
    private Vector2 mousePos;
    private Vector3 currentRot;
    private Vector3 camSmoothVelocity = Vector3.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        yRot += mousePos.x;
        xRot += mousePos.y;

        xRot = Mathf.Clamp(xRot, -40f, 40f);

        Vector3 nextRot = new Vector3(xRot, yRot);
        currentRot = Vector3.SmoothDamp(currentRot, nextRot, ref camSmoothVelocity, smoothTime);
        transform.localEulerAngles = currentRot;

        transform.position = target.position - transform.forward * distanceOffset;
    }

    public void OnLookRB(InputAction.CallbackContext value)
    {
        mousePos = value.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
    }
}
