using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStepManager : MonoBehaviour
{
    public static GameStepManager Instance;

    public List<Vector2> playerHistory = new List<Vector2>();
    public int MaxHistory = 7;

    public event System.Action OnPlayerStep;
    public event System.Action OnRewind;
    public event System.Action OnLose;
    public event System.Action OnWin;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Kill duplicate on reload
            return;
        }
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

        OnLose?.Invoke();
        //Indicate here the lose condition when player touches with enemy layermask
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload it
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoalReach()
    {
        if (SceneManager.GetActiveScene().buildIndex < 7)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            playerHistory.Clear();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
            OnWin?.Invoke();
    }

    public List<Vector2> GetHistorySnapshot()
    {
        return new List<Vector2>(playerHistory);
    }
}
