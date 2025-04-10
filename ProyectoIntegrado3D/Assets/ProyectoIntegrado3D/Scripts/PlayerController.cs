using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Stats")]
    public float speed;
    public float maxForce = 1;

    [Header("Jump Stats")]
    public float jumpForce;
    // Variables del GroundCheck:
    [SerializeField] GameObject groundCheck;
    [SerializeField] float groundCheckRadious;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool isGrounded;

    // Referencias privadas:
    private Rigidbody playerRb;
    private Animator anim;
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = GameObject.Find("GroundCheck");
    }

    private void Update()
    {
        GroundCheck();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 currentVelocity = playerRb.velocity; // Velocidad actual del player
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y); // Velocidad hacia la que queremos que se mueva el player

        // Alinear la dirección con la orientación correcta (de local a global)
        targetVelocity = transform.TransformDirection(targetVelocity);

        // Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z); // Hace que la aceleración no afecte en ve
        Vector3.ClampMagnitude(velocityChange, maxForce);

        // Aplicamos el movimiento
        playerRb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundCheckRadious, groundLayer);
    }

    void Jump()
    {
        if (isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }

    #endregion
}
