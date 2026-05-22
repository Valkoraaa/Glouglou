using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
    public static Character Instance;

    [Header("Physique & Mouvement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -20; 

    [Header("Cam�ra FPS")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 15f;
    [SerializeField] private Vector3 fpsOffset = new Vector3(0f, 1.6f, 0f);

    private CharacterController controller;
    private PlayerInputActions inputActions;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity; 
    private float xRotation = 0f;
    public bool canMove = true;
    public bool canMoveCam = true;
    public Vector3 move;
    public bool stopChara;
    public bool cinematic;
    [SerializeField] private AudioSource footstepAudio;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ApplyCameraPosition();
        HandleGravity(stopChara);
        if(canMove)
        {
            HandleMovement();
        }
        if(canMoveCam) { HandleRotation(); }
        HandleFootsteps();
    }

    private void HandleRotation()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        move = transform.right * moveInput.x + transform.forward * moveInput.y;
        
    }

    private void HandleGravity(bool hasToStop)
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        if(hasToStop) {move = Vector3.zero;}
        controller.Move(move * speed * Time.deltaTime);

        

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleFootsteps()
    {
        //bool isMoving = moveInput.magnitude > 0.1f;

        if (move!= Vector3.zero && !footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
        else if (move == Vector3.zero && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }

    private void ApplyCameraPosition()
    {
        if(!cinematic)
        {
            cameraTransform.localPosition = fpsOffset;
        }
    }

    #region Inputs
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnDisable() => inputActions.Player.Disable();
    #endregion
}