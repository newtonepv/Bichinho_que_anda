using UnityEngine;
using UnityEngine.InputSystem;

public class MoveByInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody rigidbody;


    [SerializeField]
    private float speed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody = GetComponent<Rigidbody>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }

    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }
    }

    private void Update()
    {
        
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputMovement = context.ReadValue<Vector2>();
        Debug.Log("Apertou: " + inputMovement.x + ", " + inputMovement.y);
        rigidbody.velocity = (new Vector3(inputMovement.x , 0, inputMovement.y))*speed;
    }
}
