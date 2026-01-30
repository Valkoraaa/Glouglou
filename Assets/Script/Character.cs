using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -10f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 50f;
    [SerializeField] private float transitionSpeed = 8f;

    [Header("FPS Settings")]
    [SerializeField] private Vector3 fpsOffset = new Vector3(0f, 1.6f, 0f);

    [Header("Isometric Settings")]
    [SerializeField] private Vector3 isoOffset = new Vector3(0f, 10f, -10f);
    [SerializeField] private float isoAngle = 45f;
    [SerializeField] private float isoYRotation = 45f;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;

    private bool isFPS = false;
    private float xRotation = 0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateCameraTargets();
    }

    private void Update()
    {

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isFPS = !isFPS;
            if (!isFPS)
            {
                xRotation = 0f; 
            }
        }

        if (isFPS)
        {
            HandleFPSCamera();
        }

        UpdateCameraTargets();

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * transitionSpeed);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, Time.deltaTime * transitionSpeed);

        HandleMovement();
    }

    private void HandleFPSCamera()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void UpdateCameraTargets()
    {
        if (isFPS)
        {
            targetPosition = transform.position + transform.rotation * fpsOffset;
            targetRotation = transform.rotation * Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            targetPosition = transform.position + isoOffset;
            targetRotation = Quaternion.Euler(isoAngle, isoYRotation, 0f);
        }
    }

    private void HandleMovement()
    {
        Vector3 move;

        if (isFPS)
        {
            move = transform.right * moveInput.x + transform.forward * moveInput.y;
        }
        else
        {
            move = Vector3.right * moveInput.x + Vector3.forward * moveInput.y;
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}