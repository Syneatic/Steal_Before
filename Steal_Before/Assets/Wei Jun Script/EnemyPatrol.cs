using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolType { Horizontal, Vertical }

public class EnemyPatrol : MonoBehaviour
{
    private List<Vector2> enemyHistory = new List<Vector2>();
    private List<Vector2> directionHistory = new List<Vector2>();

    public PatrolType movementType; // This shows as a dropdown in the Inspector
    public bool startPositive = true; // True = Right/Up, False = Left/Dow

    private Vector2 direction = Vector2.zero;
    public LayerMask wallLayer;

    private bool isSubscribed = false;

    private void Start()
    {
        // Determine initial direction based on the dropdown choice
        if (movementType == PatrolType.Horizontal)
        {
            direction = startPositive ? Vector2.right : Vector2.left;
        }
        else
        {
            direction = startPositive ? Vector2.up : Vector2.down;
        }

        enemyHistory.Clear();
        directionHistory.Clear();

        enemyHistory.Add(transform.position);
        directionHistory.Add(direction);

        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        if (GameStepManager.Instance != null)
        {
            GameStepManager.Instance.OnPlayerStep -= MoveEnemy;
            GameStepManager.Instance.OnRewind -= RewindEnemy;
            GameStepManager.Instance.OnLose -= EnemyTouchPlayer;
        }
        isSubscribed = false;
    }
    private void MoveEnemy()
    {
        Vector2 targetPos = (Vector2)transform.position + direction;

        if (Physics2D.OverlapCircle(targetPos, 0.3f, wallLayer))
        {
            direction *= -1;
            targetPos = (Vector2)transform.position + direction;
        }

        transform.position = targetPos;
        enemyHistory.Add(transform.position);
        directionHistory.Add(direction);

        if (enemyHistory.Count > GameStepManager.MaxHistory - 1)
        {
            enemyHistory.RemoveAt(0);
            directionHistory.RemoveAt(0);
        }

        if (Vector2.Distance(transform.position, PlayerControllerScript.pos) < 0.1f)
        {
            EnemyTouchPlayer();
        }
    }

    private void RewindEnemy()
    {
        if (enemyHistory.Count > 0)
        {
            // Jump back to the oldest position
            transform.position = enemyHistory[0];

            // Jump back to the oldest direction!
            direction = directionHistory[0];

            // Reset lists to the "new" starting point
            Vector2 currentPos = enemyHistory[0];
            Vector2 currentDir = directionHistory[0];

            enemyHistory.Clear();
            directionHistory.Clear();

            enemyHistory.Add(currentPos);
            directionHistory.Add(currentDir);
        }
    }

    public void EnemyTouchPlayer()
    {
        Debug.Log("Enemy touch player : Lose");
    }

    private void Subscribe()
    {
        if (isSubscribed || GameStepManager.Instance == null) return;

        GameStepManager.Instance.OnPlayerStep += MoveEnemy;
        GameStepManager.Instance.OnRewind += RewindEnemy;
        GameStepManager.Instance.OnLose += EnemyTouchPlayer;
        isSubscribed = true;
    }
}
