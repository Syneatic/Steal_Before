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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = pos;
        rb.position = pos;
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

    private void Moving(InputAction.CallbackContext context)
    {
        //Check if the frame is pressed
        if (!context.started) return;

        var control = context.control;
        Vector3 moveDir = Vector3.zero;

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

        transform.position += moveDir;
    }
}
