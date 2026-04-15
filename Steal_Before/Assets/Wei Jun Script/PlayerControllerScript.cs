using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody2D rb;
    Vector2 moveDirection;

    public float checkDistance = 0.5f;
    public LayerMask collisionLayer;
    public LayerMask Triggers;
    public LayerMask enemyLayer;

    //Button calling for PlayerController
    private InputSystem_Actions controls;

    //Call the script of AI
    public AIScript AIMimicScript;

    //Link list to store the data of the player movement (Vector2)
    private LinkedList<Vector2> movementHistory = new LinkedList<Vector2>();
    private int maxNode => GameStepManager.Instance.MaxHistory - 1;

    private List<Vector2> heldDirections = new List<Vector2>();

    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        movementHistory.AddLast(rb.position);
    }

    private void OnEnable()
    {
        controls.Player.Rewind.started += RewindAbility;

        controls.Player.Move.started += OnMoveStarted;
        controls.Player.Move.canceled += OnMoveCanceled;

        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Move.started -= OnMoveStarted;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Rewind.started -= RewindAbility;

        controls.Disable();
    }

    public Vector2 lastInput;

    public LinkedList<Vector2> GetMovementHistory()
    {
        return movementHistory;
    }

    private Vector2 ReadDominantDirection(InputAction.CallbackContext context)
    {
        Vector2 value = context.action.ReadValue<Vector2>();
        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
            return new Vector2(Mathf.Sign(value.x), 0);
        if (Mathf.Abs(value.y) > Mathf.Abs(value.x))
            return new Vector2(0, Mathf.Sign(value.y));
        return Vector2.zero;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        Vector2 dir = ReadDominantDirection(context);
        if (dir == Vector2.zero) return;

        // Remove if already tracked (shouldn't happen, but safety net)
        heldDirections.Remove(dir);
        // Add to end — most recently pressed
        heldDirections.Add(dir);

        MovePlayer(dir);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Vector2 dir = ReadDominantDirection(context);

        // If canceled returns zero (all keys released), clear everything
        if (dir == Vector2.zero)
        {
            heldDirections.Clear();
            return;
        }

        heldDirections.Remove(dir);

        // If another key is still held, move in that direction
        if (heldDirections.Count > 0)
        {
            Vector2 fallback = heldDirections[heldDirections.Count - 1];
            MovePlayer(fallback);
        }
    }

    private void MovePlayer(Vector2 dir)
    {
        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos + dir;

        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.3f, collisionLayer);

        if (hit != null)
        {
            Debug.Log("Hit wall");
            return;
        }

        rb.position += dir;
        transform.position = rb.position;

        if (!AIMimicScript.GetIsMimic())
        {
            movementHistory.AddLast(rb.position);

            if (movementHistory.Count > maxNode) movementHistory.RemoveFirst();
        }
        else
        {
            if (movementHistory.Count > 0)
            {
                movementHistory.AddLast(rb.position);
                movementHistory.RemoveFirst();
            }
        }

        Collider2D hitTrigger = Physics2D.OverlapCircle(transform.position, 0.3f, Triggers);

        if (hitTrigger != null)
        {
            if (hitTrigger.CompareTag("Button"))
            {
                if (hitTrigger.TryGetComponent(out TriggerScript button))
                {
                    button.PushButton();
                }
            }
            else if (hitTrigger.CompareTag("Goal"))
            {
                GameStepManager.Instance.GoalReach();
            }
        }

        // The Signal for the AI to move
        GameStepManager.Instance.RegisterStep(transform.position);

        Physics2D.SyncTransforms();

        // 2. CHECK: Did we land on the same tile?
        Collider2D hitLand = Physics2D.OverlapCircle(transform.position, 0.3f, enemyLayer);

        // 3. CHECK: Did we "Swap" places? (Did the enemy land where we just were?)
        //Collider2D hitSwap = Physics2D.OverlapCircle(startPos, 0.3f, enemyLayer);

        if (hitLand != null/* || hitSwap != null*/)
        {
            // LOSE LOGIC
            Debug.Log("Collision");
            if (hitLand.TryGetComponent(out EnemyPatrol enemy))
            {
                //enemy.EnemyTouchPlayer();

            }
            GameStepManager.Instance.TriggerTouch();
        }
    }

    private void RewindAbility(InputAction.CallbackContext context)
    {

        if (!context.started || movementHistory.Count < maxNode)
        {
            Debug.Log("Not enough steps have been recorded to rewind yet!");
            return;
        }

        GameStepManager.Instance.TriggerRewind();

        // Start the mimic behavior only when E is pressed
        List<Vector2> pathForAI = new List<Vector2>(movementHistory);

        //Make the player go back to the 5 steps
        Vector2 rewindPos = movementHistory.First.Value;

        //Reset player state
        movementHistory.Clear();
        movementHistory.AddLast(rewindPos);

        //transform.position = rewindPos;
        //rb.position = rewindPos; //Update Rigidbody too to keep them in sync

        if (AIMimicScript != null)
        {
            AIMimicScript.ActivateMimic(pathForAI);
            Debug.Log("Activating Mimic");
        }

        Debug.Log("Rewind complete. History Resets");
    }
}
