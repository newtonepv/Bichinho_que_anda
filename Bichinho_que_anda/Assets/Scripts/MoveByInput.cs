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
        
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
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
