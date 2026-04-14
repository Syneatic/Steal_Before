using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class GameStepManager : MonoBehaviour
{
    public static GameStepManager Instance;

    public List<Vector2> playerHistory = new List<Vector2>();
    public int MaxHistory = 8;

    public static event System.Action OnPlayerStep;

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

    public List<Vector2> GetHistorySnapshot()
    {
        return new List<Vector2>(playerHistory);
    }
}
