using UnityEngine;
using UnityEngine.InputSystem; // Add this namespace

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // New Input System syntax
        // This reads WASD / Arrow keys automatically if "Keyboard" is present
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) {   
                input.y = 1; 
            }
            if (Keyboard.current.sKey.isPressed) {
                input.y = -1;
            }

            if (Keyboard.current.aKey.isPressed) {
                input.x = -1; 
            }
            if (Keyboard.current.dKey.isPressed) {
                input.x = 1;
            }
        }

        movement = input.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
