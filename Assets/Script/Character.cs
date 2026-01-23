using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -10f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform; 

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;

    private float xRotation = 0f;

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
        // Verrouille la souris au centre de l'écran et la rend invisible
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void Update()
    {

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);


        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }
    }
}
