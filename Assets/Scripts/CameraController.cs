using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerCam;

    public float mouseSensitivity = 1;
    private float xRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        Vector2 mousePos = value.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mousePos.x);

        // Clamp prevents over-rotating, which could lead to looking behind the player
        xRot = Mathf.Clamp(xRot - mousePos.y, -90f, 90f);
        playerCam.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
}
