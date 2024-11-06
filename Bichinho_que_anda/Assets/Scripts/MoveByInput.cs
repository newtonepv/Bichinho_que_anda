using System.Security.Cryptography;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveByInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rigidbodyDoBicho;


    [SerializeField]
    private float speed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbodyDoBicho = GetComponent<Rigidbody>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

    private void Start()
    {
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[16];  // 16 bytes = 128 bits
            rng.GetBytes(randomBytes);

            Debug.Log("Cryptographically secure random bytes:");
            foreach (var b in randomBytes)
            {
                Debug.Log(b + " ");
            }
        }
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        Debug.Log("Apertou: " + inputMovement.x + ", " + inputMovement.y);
        rigidbodyDoBicho.velocity = (new Vector3(inputMovement.x, 0, inputMovement.y)) * speed;
    }
    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }
    }
}
