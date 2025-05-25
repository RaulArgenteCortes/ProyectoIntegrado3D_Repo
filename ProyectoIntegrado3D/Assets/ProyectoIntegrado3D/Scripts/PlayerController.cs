using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Stats")]
    public float speed;
    public float maxForce = 1;
    [SerializeField] GameObject playerBody; // Cuerpo del jugador AKA Parte visible del jugador
    [SerializeField] float rotationSpeed;

    [Header("Jump Stats")]
    public float jumpForce;
    // Variables del GroundCheck:
    [SerializeField] GameObject groundCheck;
    [SerializeField] float groundCheckRadious;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] bool isGrounded;

    [Header("Light Stats")]
    public bool canConcealLight;
    public bool isLightConcealed;
    GameObject playerLight;
    // Estados de luz:
    public float unconcealedLight;
    public float concealedLight;

    // Referencias privadas:
    private Rigidbody playerRb;
    private Animator anim;
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        anim = GameObject.Find("Body").GetComponent<Animator>();
        groundCheck = GameObject.Find("GroundCheck");
        playerLight = GameObject.Find("PlayerLight");
        playerBody = GameObject.Find("Body");
    }

    private void Start()
    {
        // Estableze las variables de luz.
        canConcealLight = true;
        isLightConcealed = false;
        playerLight.GetComponent<Light>().intensity = unconcealedLight * 1;
        playerLight.GetComponent<Light>().range = unconcealedLight;   
    }

    private void Update()
    {
        GroundCheck();

        // Previene que el cuerpo del jugador rote verticalmente
        playerBody.transform.eulerAngles = new Vector3(0, playerBody.transform.eulerAngles.y, 0);

        if (transform.position.y < -5)
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 currentVelocity = playerRb.velocity; // Velocidad actual del jugador.
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y); // Velocidad hacia la que queremos que se mueva el jugador.
        targetVelocity *= speed; // Aplica la velocidad hacia la que queremos que se mueva el jugador.

        // Alinear la dirección con la orientación correcta (de local a global)
        targetVelocity = transform.TransformDirection(targetVelocity);

        // Calcular las fuerzas que afectan al movimiento
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z); // Hace que la aceleración no afecte en ve
        Vector3.ClampMagnitude(velocityChange, maxForce);

        // Aplicamos el movimiento
        playerRb.AddForce(velocityChange, ForceMode.VelocityChange);

        // Rotacion del cuerpo
        if (currentVelocity != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(currentVelocity, Vector3.up);

            playerBody.transform.rotation = Quaternion.RotateTowards(playerBody.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
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

    void ConcealLight() // Cambia la intensidad de la luz.
    {
        canConcealLight = false;
        
        isLightConcealed = !isLightConcealed;

        playerLight.GetComponent<Light>().range = isLightConcealed ? // Cambia la variable dependiendo si el booleano es "true" o "false".
            concealedLight : // Le da este valor a la variable si es "true".
            unconcealedLight ; // Le da este valor a la variable si es "false".

        playerLight.GetComponent<Light>().intensity = isLightConcealed ?
            concealedLight * 1 :
            unconcealedLight * 1 ;

        canConcealLight = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    #region Input Methods

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }

    public void OnConcealLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canConcealLight == true) 
            {
                ConcealLight();
            }
        }
    }

    #endregion
}
