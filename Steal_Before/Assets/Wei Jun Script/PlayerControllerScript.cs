using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControllerScript : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 moveDirection;
    public static Vector2 pos = new Vector2(0.5f, 0.5f);

    //Button calling for PlayerController
    public InputAction playerController;

    //Call the script of AI
    public AIScript AIMimicScript;

    private LinkedList<Vector2> movementHistory = new LinkedList<Vector2>();
    private LinkedListNode<Vector2> currentNode;
    private int maxNode = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = pos;
        rb.position = pos;
        currentNode = movementHistory.AddLast(rb.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        playerController.Enable();
        playerController.started += Moving;


    }

    private void OnDisable()
    {
        playerController.started -= Moving;
        playerController.Disable();
    }

    public Vector2 lastInput;

    public LinkedList<Vector2> GetMovementHistory()
    {
        return movementHistory;
    }

    private void Moving(InputAction.CallbackContext context)
    {
        //Check if the frame is pressed
        if (!context.started) return;

        var control = context.control;
        Vector2 moveDir = Vector2.zero;

        float controlValue = context.ReadValue<Vector2>().x != 0 ? context.ReadValue<Vector2>().x : context.ReadValue<Vector2>().y;

        if (control.name == "a" || control.name == "d")
        {
            // Move horizontal based on THIS key's value only
            float x = (control.name == "a") ? -1 : 1;
            moveDir = new Vector3(x, 0, 0);
        }
        else if (control.name == "w" || control.name == "s")
        {
            // Move vertical based on THIS key's value only
            float y = (control.name == "s") ? -1 : 1;
            moveDir = new Vector3(0, y, 0);
        }

        if (moveDir != Vector2.zero)
        {
            rb.position += moveDir;
            transform.position = rb.position;

            currentNode = movementHistory.AddLast(rb.position);

            if (movementHistory.Count > maxNode) movementHistory.RemoveFirst();

            if (AIMimicScript != null)
            {
                AIMimicScript.PlayerMove();
            }

                Debug.Log("History Size: " + movementHistory.Count);
        }
    }
}
