using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public Transform cameraTransform;
    public Rigidbody rb;
    public GameObject glowStick;
    public Transform throwPoint;
    public float throwForce = 10f;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction throwAction;
    private float xRotation = 0f;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions.FindAction("Move");
            lookAction = playerInput.actions.FindAction("Look");
            throwAction = playerInput.actions.FindAction("Interact");
        }

        moveAction.Enable();
        lookAction.Enable();
        throwAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        throwAction.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    private void Update()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * lookSpeed;
        float mouseY = lookInput.y * lookSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (throwAction.WasPressedThisFrame())
        {
            ThrowGlowStick();
        }
    }

    private void ThrowGlowStick()
    {
        GameObject thrownGlowStick = Instantiate(glowStick, throwPoint.position, throwPoint.rotation);
        Rigidbody glowStickRb = thrownGlowStick.GetComponent<Rigidbody>();
        if (glowStickRb != null)
        {
            Vector3 playerVelocity = rb.linearVelocity;
            glowStickRb.linearVelocity = playerVelocity; // Inherit player's velocity
            glowStickRb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        }
    }
}
