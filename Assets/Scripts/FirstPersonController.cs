using System.Net.Http.Headers;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{

    [SerializeField] private Transform cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 15f;   // for game - 4
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float g = -9.81f;

    [Header("Camera")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 90f;


    private CharacterController controller;
    private float verticalVelocity;
    private float xRotation = 0f;


    /*                        UNDERWATER                  LAND
            walkSpeed = 2-2.5                           | 4
            g = -1 / -2 (0 = neutral)                   | -9.81
            max brzina tonjenja = -2 do -3
            jumpHeight = 0.3-0.5 / ukloniti             | 1
            vertikalno plivanje (Space/Ctrl) = 1.5 - 2
            drag (otpor vode) = 2-4
            grounded verticalVelocity = 0               | -2
            mouseSensitivity = 1.5  (1-2)               | 2
            maxLookAngle = 90 (ili 85)
    */

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null) // Check if cameraTransform is not assigned and Camera.main is available
        {
            cameraTransform = Camera.main.transform;
        }

    }
    void Start()
    {
        LockCursor(true);
    }

    void Update()
    {
        HandleCursor();

        if (Cursor.lockState != CursorLockMode.Locked) { 
            return; // Skip movement and camera handling if the cursor is not locked
        }

        HandleLook();
        HandleMovement();

    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");  // A/D
        float vertical = Input.GetAxis("Vertical");   // W/S

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        move = Vector3.ClampMagnitude(move, 1f); // Normalize diagonal movement

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Small negative value to keep the player grounded
        }
        if (Input.GetButtonDown("Jump") && controller.isGrounded) // space
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * g);
        }
        verticalVelocity += g * Time.deltaTime;

        Vector3 velocity = move * walkSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

    }

    private void HandleCursor()
    {
        // Lock the cursor to the center of the screen and hide it
        if (Input.GetKeyDown(KeyCode.Escape))
       {
            LockCursor(false);
        }
       else if (Input.GetMouseButtonDown(0))
       {
           LockCursor(true);
        }
    }

    private void LockCursor(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // left/right rotation (yaw) is applied to the player object
        transform.Rotate(Vector3.up * mouseX);
        
        // up/down rotation (pitch) is applied to the camera
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
