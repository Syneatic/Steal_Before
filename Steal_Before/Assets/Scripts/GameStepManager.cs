using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStepManager : MonoBehaviour
{
    public static GameStepManager Instance;

    public List<Vector2> playerHistory = new List<Vector2>();
    public int MaxHistory { get; set; } = 7;

    public event System.Action OnPlayerStep;
    public event System.Action OnRewind;
    public event System.Action OnLose;
    public event System.Action OnWin;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterStep(Vector2 newPos)
    {
        playerHistory.Add(newPos);

        if (playerHistory.Count > MaxHistory) playerHistory.RemoveAt(0);

        OnPlayerStep?.Invoke();
    }

    public void TriggerRewind() { OnRewind?.Invoke(); }
    public void TriggerTouch() {
        //Indicate here the lose condition when player touches with enemy layermask
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload it
        SceneManager.LoadScene(currentSceneIndex);

        OnLose?.Invoke(); 
    }

    public void GoalReach()
    {
        OnWin?.Invoke();
    }

    public List<Vector2> GetHistorySnapshot()
    {
        return new List<Vector2>(playerHistory);
    }
}
